using AutoMapper;
using Core.IServices;
using Core.Models;
using Core.Utils;
using Data.Entities;
using Data.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Service class for managing articles.
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserService _IUserService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleService"/> class.
        /// </summary>
        /// <param name="articleRepository">The article repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="fileService">The file service for handling images.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="tagRepository">The tag repository.</param>
        /// <param name="userService">The user service.</param>
        public ArticleService(IArticleRepository articleRepository, IMapper mapper,
                              IFileService fileService, IUserRepository userRepository,
                              ITagRepository tagRepository, IUserService userService)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _fileService = fileService;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _IUserService = userService;
        }

        /// <summary>
        /// Creates a new article asynchronously.
        /// </summary>
        /// <param name="articleForCreationDto">The article data transfer object containing article creation data.</param>
        /// <param name="username">The username of the user creating the article.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="ArticleDto"/> as the result.</returns>
        /// <exception cref="Exception">Thrown when the image file is required but not provided.</exception>
        public async Task<ArticleDto> CreateArticleAsync(ArticleForCreationDto articleForCreationDto, string username)
        {
            var user = await GetUserAsync(username);
            var article = MapToArticle(articleForCreationDto, user);
            await HandleImageAsync(articleForCreationDto.image, article);
            await HandleTagsAsync(articleForCreationDto.tags, article);
            await SaveArticleAsync(article);

            return await GetArticleDtoAsync(article.id);
        }

        /// <summary>
        /// Retrieves an article by its ID asynchronously.
        /// </summary>
        /// <param name="articleId">The ID of the article.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="ArticleDto"/> as the result.</returns>
        /// <exception cref="Exception">Thrown when the article is not found.</exception>
        public async Task<ArticleDto> GetArticleByIdAsync(int articleId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);

            if (article == null)
            {
                throw new Exception("Article not found.");
            }

            var articleDto = _mapper.Map<ArticleDto>(article);
            articleDto.likes_count = article.article_likes.Count;

            var comments = await _articleRepository.GetCommentsByArticleIdAsync(articleId);
            articleDto.comments = _mapper.Map<List<ArticleCommentDto>>(comments);

            return articleDto;
        }

        /// <summary>
        /// Edits an existing article asynchronously.
        /// </summary>
        /// <param name="articleId">The ID of the article to edit.</param>
        /// <param name="articleForUpdateDto">The data transfer object containing updated article data.</param>
        /// <param name="username">The username of the user editing the article.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="ArticleDto"/> as the result.</returns>
        /// <exception cref="Exception">Thrown when the article is not found or user is not authorized.</exception>
        public async Task<ArticleDto> EditArticleAsync(int articleId, ArticleForUpdateDto articleForUpdateDto, string username)
        {
            var user = await GetUserAsync(username);
            var existingArticle = await GetExistingArticleAsync(articleId);

            UpdateArticleProperties(existingArticle, articleForUpdateDto);
            await HandleImageAsync(articleForUpdateDto.image, existingArticle);
            await UpdateTagsAsync(articleForUpdateDto.tags, existingArticle);
            _articleRepository.Update(existingArticle);
            await SaveChangesAsync();

            return _mapper.Map<ArticleDto>(existingArticle);
        }

        /// <summary>
        /// Deletes an article asynchronously.
        /// </summary>
        /// <param name="articleId">The ID of the article to delete.</param>
        /// <param name="username">The username of the user requesting the deletion.</param>
        /// <returns>A task that represents the asynchronous operation, with a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when the article is not found or user is not authorized.</exception>
        public async Task<bool> DeleteArticleAsync(int articleId, string username)
        {
            var article = await GetExistingArticleAsync(articleId);
            await EnsureUserIsOwnerAsync(article, username);

            _articleRepository.Delete(article);
            await SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Ensures that the user is the owner of the article.
        /// </summary>
        /// <param name="article">The article to check ownership of.</param>
        /// <param name="username">The username of the user.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this action on the article.</exception>
        private async Task EnsureUserIsOwnerAsync(Article article, string username)
        {
            var user = await _userRepository.GetUserAsNoTrackingAsync(username);
            if (article.user_id != user.id)
            {
                throw new UnauthorizedAccessException("User not authorized to perform this action on the article.");
            }
        }

        /// <summary>
        /// Retrieves a user by username asynchronously.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="User"/> as the result.</returns>
        /// <exception cref="Exception">Thrown when the user does not exist.</exception>
        private async Task<User> GetUserAsync(string username)
        {
            return await _userRepository.GetUserAsNoTrackingAsync(username)
                   ?? throw new Exception("User does not exist.");
        }

        /// <summary>
        /// Maps the article creation data transfer object to an Article entity.
        /// </summary>
        /// <param name="articleForCreationDto">The article creation DTO.</param>
        /// <param name="user">The user creating the article.</param>
        /// <returns>The mapped <see cref="Article"/> entity.</returns>
        private Article MapToArticle(ArticleForCreationDto articleForCreationDto, User user)
        {
            var article = _mapper.Map<Article>(articleForCreationDto);
            article.user_id = user.id;
            article.slug = GenerateSlug(article.title, article.id);
            article.created = DateTime.UtcNow;

            return article;
        }

        /// <summary>
        /// Generates a slug from the article title.
        /// </summary>
        /// <param name="title">The title of the article.</param>
        /// <param name="articleId">The ID of the article.</param>
        /// <returns>A slug for the article.</returns>
        private string GenerateSlug(string title, int articleId)
        {
            return !string.IsNullOrWhiteSpace(title)
                ? Slug.GenerateSlug(title, title, articleId)
                : string.Empty;
        }

        /// <summary>
        /// Handles the image upload for the article.
        /// </summary>
        /// <param name="imageFile">The image file to upload.</param>
        /// <param name="article">The article to associate with the uploaded image.</param>
        /// <exception cref="Exception">Thrown when the image file is required but not provided.</exception>
        private async Task HandleImageAsync(IFormFile imageFile, Article article)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                article.image = await _fileService.SaveFileAsync(imageFile);
            }
            else
            {
                throw new Exception("Image file is required.");
            }
        }

        /// <summary>
        /// Handles the tags associated with the article.
        /// </summary>
        /// <param name="tags">The list of tags to associate with the article.</param>
        /// <param name="article">The article to associate the tags with.</param>
        private async Task HandleTagsAsync(List<string> tags, Article article)
        {
            if (tags != null && tags.Any())
            {
                var tagsFromDb = await _tagRepository.GetTagsByNamesAsync(tags);
                if (tagsFromDb != null && tagsFromDb.Any())
                {
                    article.article_tags.AddRange(tagsFromDb.Select(tag => CreateArticleTag(article, tag)));
                }
            }
        }

        /// <summary>
        /// Creates an ArticleTag entity for associating an article with a tag.
        /// </summary>
        /// <param name="article">The article associated with the tag.</param>
        /// <param name="tag">The tag to associate with the article.</param>
        /// <returns>A new <see cref="ArticleTag"/> entity.</returns>
        private ArticleTag CreateArticleTag(Article article, Tag tag)
        {
            return new ArticleTag
            {
                article = article,
                tag = tag,
                created = DateTime.UtcNow,
                updated = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Saves the new article to the repository.
        /// </summary>
        /// <param name="article">The article to save.</param>
        private async Task SaveArticleAsync(Article article)
        {
            await _articleRepository.AddAsync(article);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves articles based on search criteria asynchronously.
        /// </summary>
        /// <param name="searchDto">The search criteria.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="ArticleCardDto"/> as the result.</returns>
        public async Task<IEnumerable<ArticleCardDto>> GetArticlesAsync(ArticlesSearchDto searchDto)
        {
            var articles = await _articleRepository.GetArticlesAsync(searchDto.Offset, searchDto.keyword, searchDto.tag);

            if (!articles.Any())
                return Enumerable.Empty<ArticleCardDto>();

            return articles.Select(a => _mapper.Map<ArticleCardDto>(a));
        }

        /// <summary>
        /// Retrieves an <see cref="ArticleDto"/> for a given article ID.
        /// </summary>
        /// <param name="articleId">The ID of the article.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="ArticleDto"/> as the result.</returns>
        private async Task<ArticleDto> GetArticleDtoAsync(int articleId)
        {
            var savedArticle = await _articleRepository.GetArticleByIdAsync(articleId);
            return _mapper.Map<ArticleDto>(savedArticle);
        }

        /// <summary>
        /// Retrieves an existing article by its ID.
        /// </summary>
        /// <param name="articleId">The ID of the article.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="Article"/> as the result.</returns>
        /// <exception cref="Exception">Thrown when the article does not exist.</exception>
        private async Task<Article> GetExistingArticleAsync(int articleId)
        {
            return await _articleRepository.GetArticleByIdAsync(articleId)
                   ?? throw new Exception("Article does not exist.");
        }

        /// <summary>
        /// Updates the properties of an existing article.
        /// </summary>
        /// <param name="existingArticle">The existing article to update.</param>
        /// <param name="articleForUpdateDto">The data transfer object containing updated article data.</param>
        private void UpdateArticleProperties(Article existingArticle, ArticleForUpdateDto articleForUpdateDto)
        {
            existingArticle.title = articleForUpdateDto.title;
            existingArticle.body = articleForUpdateDto.body;
            existingArticle.updated = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the tags associated with an existing article.
        /// </summary>
        /// <param name="tags">The list of tags to associate with the article.</param>
        /// <param name="existingArticle">The article to associate the tags with.</param>
        private async Task UpdateTagsAsync(List<string> tags, Article existingArticle)
        {
            existingArticle.article_tags.Clear();

            if (tags != null && tags.Any())
            {
                var tagsFromDb = await _tagRepository.GetTagsByNamesAsync(tags);
                existingArticle.article_tags.AddRange(tagsFromDb.Select(tag => CreateArticleTag(existingArticle, tag)));
            }
        }

        /// <summary>
        /// Adds a comment to an article asynchronously.
        /// </summary>
        /// <param name="articleId">The ID of the article to comment on.</param>
        /// <param name="commentDto">The comment data transfer object.</param>
        /// <param name="username">The username of the user adding the comment.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="ArticleCommentDto"/> as the result.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the article is not found.</exception>
        /// <exception cref="Exception">Thrown when the user is not found.</exception>
        public async Task<ArticleCommentDto> AddCommentToArticleAsync(int articleId, ArticleCommentDto commentDto, string username)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null)
            {
                throw new KeyNotFoundException("Article not found.");
            }

            var user = await _userRepository.GetUserAsNoTrackingAsync(username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var comment = new ArticleComment
            {
                article_id = articleId,
                user_id = user.id,
                body = commentDto.body,
                created = DateTime.UtcNow
            };

            await _articleRepository.AddCommentAsync(comment);
            await _articleRepository.SaveChangesAsync();

            return new ArticleCommentDto
            {
                body = comment.body,
                user_first_name = user.first_name,
                user_last_name = user.last_name,
                created = comment.created
            };
        }

        /// <summary>
        /// Saves changes made to the repository asynchronously.
        /// </summary>
        /// <exception cref="DbUpdateException">Thrown when there is an issue with saving changes to the database.</exception>
        /// <exception cref="Exception">Thrown for general exceptions.</exception>
        private async Task SaveChangesAsync()
        {
            try
            {
                await _articleRepository.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                HandleDbUpdateException(dbEx);
            }
            catch (Exception ex)
            {
                HandleGeneralException(ex);
            }
        }

        /// <summary>
        /// Handles database update exceptions.
        /// </summary>
        /// <param name="dbEx">The database update exception.</param>
        private void HandleDbUpdateException(DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
            }
        }

        /// <summary>
        /// Handles general exceptions.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        private void HandleGeneralException(Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }

        /// <summary>
        /// Likes an article asynchronously.
        /// </summary>
        /// <param name="slug">The slug of the article to like.</param>
        /// <returns>A task that represents the asynchronous operation, with a boolean indicating success.</returns>
        public async Task<bool> LikeArticleAsync(string slug)
        {
            var article = await _articleRepository.GetArticleAsync(slug);
            if (article == null)
            {
                return false;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();
            var isLiked = await _articleRepository.IsLikedAsync(currentUserId, article.id);
            if (isLiked)
            {
                return false;
            }

            await _articleRepository.LikeArticleAsync(currentUserId, article.id);
            await _articleRepository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Unlikes an article asynchronously.
        /// </summary>
        /// <param name="slug">The slug of the article to unlike.</param>
        /// <returns>A task that represents the asynchronous operation, with a boolean indicating success.</returns>
        public async Task<bool> UnLikeArticleAsync(string slug)
        {
            var article = await _articleRepository.GetArticleAsync(slug);
            if (article == null)
            {
                return false;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();
            var isLiked = await _articleRepository.IsLikedAsync(currentUserId, article.id);
            if (!isLiked)
            {
                return false;
            }

            _articleRepository.UnLikeArticle(currentUserId, article.id);
            await _articleRepository.SaveChangesAsync();
            return true;
        }
    }
}
