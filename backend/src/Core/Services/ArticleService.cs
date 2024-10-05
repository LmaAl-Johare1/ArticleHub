using AutoMapper;
using Core.IServices;
using Core.Models;
using Core.Utils;
using Data.Entities;
using Data.IRepositories;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly  IUserService _IUserService;

        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;

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

        public async Task<ArticleDto> CreateArticleAsync(ArticleForCreationDto articleForCreationDto, string username)
        {
            var user = await GetUserAsync(username);
            var article = MapToArticle(articleForCreationDto, user);
            await HandleImageAsync(articleForCreationDto.image, article);
            await HandleTagsAsync(articleForCreationDto.tags, article);
            await SaveArticleAsync(article);

            return await GetArticleDtoAsync(article.id);
        }
        public async Task<ArticleDto> GetArticleByIdAsync(int articleId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);

            if (article == null)
            {
                throw new Exception("Article not found.");
            }

            var articleDto = _mapper.Map<ArticleDto>(article);

            return articleDto;
        }
        public async Task<ArticleDto> EditArticleAsync(int articleId, ArticleForUpdateDto articleForUpdateDto, string username)
        {
            var user = await GetUserAsync(username);
            var existingArticle = await GetExistingArticleAsync(articleId);


            UpdateArticleProperties(existingArticle, articleForUpdateDto);
            await HandleImageAsync(articleForUpdateDto.image, existingArticle);
            await UpdateTagsAsync(articleForUpdateDto.tags, existingArticle);
             _articleRepository.Update(existingArticle);
            await SaveChangesAsync();
             var articleDto = _mapper.Map<ArticleDto>(existingArticle);;
            return (articleDto);
        }

        private async Task<User> GetUserAsync(string username)
        {
            return await _userRepository.GetUserAsNoTrackingAsync(username)
                   ?? throw new Exception("User does not exist.");
        }

        private Article MapToArticle(ArticleForCreationDto articleForCreationDto, User user)
        {
            var article = _mapper.Map<Article>(articleForCreationDto);
            article.user_id = user.id;
            article.slug = GenerateSlug(article.title, article.id);
            article.created = DateTime.UtcNow;

            return article;
        }

        private string GenerateSlug(string title, int articleId)
        {
            return !string.IsNullOrWhiteSpace(title)
                ? Slug.GenerateSlug(title, title, articleId)
                : string.Empty;
        }

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

        private async Task SaveArticleAsync(Article article)
        {
            await _articleRepository.AddAsync(article);
            await SaveChangesAsync();
        }

        private async Task<ArticleDto> GetArticleDtoAsync(int articleId)
        {
            var savedArticle = await _articleRepository.GetArticleByIdAsync(articleId);
            return _mapper.Map<ArticleDto>(savedArticle);
        }

        private async Task<Article> GetExistingArticleAsync(int articleId)
        {
            return await _articleRepository.GetArticleByIdAsync(articleId)
                   ?? throw new Exception("Article does not exist.");
        }



        private void UpdateArticleProperties(Article existingArticle, ArticleForUpdateDto articleForUpdateDto)
        {
            existingArticle.title = articleForUpdateDto.title;
            existingArticle.body = articleForUpdateDto.body;
            existingArticle.updated = DateTime.UtcNow;
        }

        private async Task UpdateTagsAsync(List<string> tags, Article existingArticle)
        {
            existingArticle.article_tags.Clear();

            if (tags != null && tags.Any())
            {
                var tagsFromDb = await _tagRepository.GetTagsByNamesAsync(tags);
                existingArticle.article_tags.AddRange(tagsFromDb.Select(tag => CreateArticleTag(existingArticle, tag)));
            }
        }

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

        private void HandleDbUpdateException(DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
            }

        }

        private void HandleGeneralException(Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");

        }

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
