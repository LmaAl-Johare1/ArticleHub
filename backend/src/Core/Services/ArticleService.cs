using AutoMapper;
using Core.IServices;
using Core.Models;
using Core.Utils;
using Data.Entities;
using Data.IRepositories;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Service for managing articles, including creation, updating, fetching, and deletion.
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<ArticleService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleService"/> class.
        /// </summary>
        /// <param name="articleRepository">The article repository.</param>
        /// <param name="mapper">The mapper service for DTO mapping.</param>
        /// <param name="fileService">Service for handling file operations (image uploads).</param>
        /// <param name="userRepository">Repository for user operations.</param>
        /// <param name="tagRepository">Repository for tag operations.</param>
        /// <param name="logger">Logger for logging errors and information.</param>
        public ArticleService(IArticleRepository articleRepository, IMapper mapper,
                              IFileService fileService, IUserRepository userRepository,
                              ITagRepository tagRepository, ILogger<ArticleService> logger)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _fileService = fileService;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new article asynchronously.
        /// </summary>
        /// <param name="articleForCreationDto">DTO containing article creation data.</param>
        /// <param name="username">The username of the author.</param>
        /// <returns>The created <see cref="ArticleDto"/>.</returns>
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
        /// <param name="articleId">The ID of the article to retrieve.</param>
        /// <returns>The article DTO if found.</returns>
        public async Task<ArticleCardDto> GetArticleByIdAsync(int articleId)
        {
            var article = await _articleRepository.GetArticleDetailsAsync(articleId);

            if (article == null)
            {
                throw new Exception("Article not found.");
            }

            // Use AutoMapper to map the article to ArticleCardDto
            return _mapper.Map<ArticleCardDto>(article);
        }

        /// <summary>
        /// Updates an existing article asynchronously.
        /// </summary>
        /// <param name="articleId">The ID of the article to update.</param>
        /// <param name="articleForUpdateDto">DTO containing updated article data.</param>
        /// <param name="username">The username of the author.</param>
        /// <returns>The updated <see cref="ArticleDto"/>.</returns>
        public async Task<ArticleDto> EditArticleAsync(int articleId, ArticleForUpdateDto articleForUpdateDto, string username)
        {
            var user = await GetUserAsync(username);
            var existingArticle = await GetExistingArticleAsync(articleId);
            await EnsureUserIsOwnerAsync(existingArticle, username);

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
        /// <param name="username">The username of the user attempting to delete the article.</param>
        /// <returns>A boolean indicating whether the article was deleted successfully.</returns>
        public async Task<bool> DeleteArticleAsync(int articleId, string username)
        {
            var article = await GetExistingArticleAsync(articleId);
            await EnsureUserIsOwnerAsync(article, username);

            _articleRepository.Delete(article);
            await SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Ensures that the specified user is the owner of the article.
        /// </summary>
        /// <param name="article">The article to check ownership of.</param>
        /// <param name="username">The username of the user.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown if the user is not the owner.</exception>
        private async Task EnsureUserIsOwnerAsync(Article article, string username)
        {
            var user = await _userRepository.GetUserAsNoTrackingAsync(username);
            if (article.user_id != user.id)
            {
                throw new UnauthorizedAccessException("User not authorized to perform this action on the article.");
            }
        }

        /// <summary>
        /// Gets the user by their username asynchronously.
        /// </summary>
        /// <param name="username">The username to lookup.</param>
        /// <returns>The user entity.</returns>
        private async Task<User> GetUserAsync(string username)
        {
            return await _userRepository.GetUserAsNoTrackingAsync(username)
                   ?? throw new Exception("User does not exist.");
        }

        /// <summary>
        /// Maps the <see cref="ArticleForCreationDto"/> to an <see cref="Article"/> entity.
        /// </summary>
        /// <param name="articleForCreationDto">DTO containing article creation data.</param>
        /// <param name="user">The user entity creating the article.</param>
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
        /// Generates a slug for an article.
        /// </summary>
        /// <param name="title">The title of the article.</param>
        /// <param name="articleId">The ID of the article.</param>
        /// <returns>A slug based on the title and ID.</returns>
        private string GenerateSlug(string title, int articleId)
        {
            return !string.IsNullOrWhiteSpace(title)
                ? Slug.GenerateSlug(title, title, articleId)
                : string.Empty;
        }

        /// <summary>
        /// Handles image file uploads and assigns the image path to the article.
        /// </summary>
        /// <param name="imageFile">The image file to upload.</param>
        /// <param name="article">The article to assign the image to.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if the image is invalid.</exception>
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
        /// Retrieves a paginated list of articles asynchronously.
        /// </summary>
        /// <param name="offset">The offset for pagination.</param>
        /// <param name="keyword">Optional keyword to filter articles.</param>
        /// <param name="tag">Optional tag to filter articles.</param>
        /// <returns>A list of <see cref="ArticleDto"/> objects.</returns>
        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync(ArticlesSearchDto searchDto)
        {

            var articles = await _articleRepository.GetArticlesAsync(searchDto.Offset,searchDto.keyword,searchDto.tag);

            if (!articles.Any())
                return Enumerable.Empty<ArticleDto>();

            return articles.Select(a => _mapper.Map<ArticleDto>(a));
        }

        /// <summary>
        /// Handles the association of tags with an article.
        /// </summary>
        /// <param name="tags">The list of tag names.</param>
        /// <param name="article">The article to associate tags with.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
        /// Creates an <see cref="ArticleTag"/> entity associating an article with a tag.
        /// </summary>
        /// <param name="article">The article entity.</param>
        /// <param name="tag">The tag entity.</param>
        /// <returns>The created <see cref="ArticleTag"/> entity.</returns>
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
        /// Saves the article to the database.
        /// </summary>
        /// <param name="article">The article entity to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SaveArticleAsync(Article article)
        {
            await _articleRepository.AddAsync(article);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves an article DTO by its ID.
        /// </summary>
        /// <param name="articleId">The article ID.</param>
        /// <returns>The corresponding <see cref="ArticleDto"/>.</returns>
        private async Task<ArticleDto> GetArticleDtoAsync(int articleId)
        {
            var savedArticle = await _articleRepository.GetArticleDetailsAsync(articleId);
            return _mapper.Map<ArticleDto>(savedArticle);
        }

        /// <summary>
        /// Retrieves an existing article by its ID asynchronously.
        /// </summary>
        /// <param name="articleId">The article ID.</param>
        /// <returns>The corresponding <see cref="Article"/> entity.</returns>
        private async Task<Article> GetExistingArticleAsync(int articleId)
        {
            return await _articleRepository.GetArticleDetailsAsync(articleId)
                   ?? throw new Exception("Article does not exist.");
        }

        /// <summary>
        /// Updates the properties of an existing article.
        /// </summary>
        /// <param name="existingArticle">The existing article entity.</param>
        /// <param name="articleForUpdateDto">DTO containing updated article data.</param>
        private void UpdateArticleProperties(Article existingArticle, ArticleForUpdateDto articleForUpdateDto)
        {
            existingArticle.title = articleForUpdateDto.title;
            existingArticle.body = articleForUpdateDto.body;
            existingArticle.updated = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the tags associated with an article.
        /// </summary>
        /// <param name="tags">The list of tag names.</param>
        /// <param name="existingArticle">The existing article entity.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
        /// Saves changes to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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
        /// <param name="dbEx">The <see cref="DbUpdateException"/> exception.</param>
        private void HandleDbUpdateException(DbUpdateException dbEx)
        {
            _logger.LogError($"Database Update Error: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                _logger.LogError($"Inner Exception: {dbEx.InnerException.Message}");
            }
        }

        /// <summary>
        /// Handles general exceptions.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> exception.</param>
        private void HandleGeneralException(Exception ex)
        {
            _logger.LogError($"General Error: {ex.Message}");
        }
    }
}
