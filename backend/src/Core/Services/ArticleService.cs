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
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper,
                              IFileService fileService, IUserRepository userRepository,
                              ITagRepository tagRepository)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _fileService = fileService;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
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
        }

        private async Task HandleTagsAsync(List<string> tags, Article article)
        {
            if (tags != null && tags.Any())
            {
                var tagsFromDb = await _tagRepository.GetTagsByNamesAsync(tags);
                if (tagsFromDb != null && tagsFromDb.Any())
                {
                    foreach (var tag in tagsFromDb)
                    {
                        article.article_tags.Add(CreateArticleTag(article, tag));
                    }
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

            try
            {
                await _articleRepository.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database Update Error: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
                }
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw; 
            }
        }

        private async Task<ArticleDto> GetArticleDtoAsync(int articleId)
        {
            var savedArticle = await _articleRepository.GetArticleByIdAsync(articleId);
            return _mapper.Map<ArticleDto>(savedArticle);
        }
    }
}
