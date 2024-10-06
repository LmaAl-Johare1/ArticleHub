using AutoMapper;
using Core.Models;
using Data.Entities;
using System;
using System.Linq;

namespace Core.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="Article"/> and related DTOs.
    /// </summary>
    public class ArticleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleProfile"/> class.
        /// Configures mappings for article-related data transfer objects (DTOs).
        /// </summary>
        public ArticleProfile()
        {
            // Mapping configuration from Article entity to ArticleCardDto.
            CreateMap<Article, ArticleCardDto>()
                .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.title))
                .ForMember(dest => dest.body, opt => opt.MapFrom(src => src.body))
                .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.image))
                .ForMember(dest => dest.likes_count, opt => opt.MapFrom(src => src.article_likes.Count()));

            // Mapping configuration for ArticleForCreationDto to Article entity.
            CreateMap<ArticleForCreationDto, Article>()
                .ForMember(dest => dest.image, opt => opt.Ignore()) // Image will be handled separately.
                .ForMember(dest => dest.created, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.updated, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping configuration from Article entity to ArticleDto.
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.tags, opt =>
                    opt.MapFrom(src => src.article_tags.Select(at => at.tag.name).ToList()))
                .ForMember(dest => dest.user_first_name, opt => opt.MapFrom(src => src.user.first_name))
                .ForMember(dest => dest.user_last_name, opt => opt.MapFrom(src => src.user.last_name));

            // Mapping configuration from ArticleComment entity to ArticleCommentDto.
            CreateMap<ArticleComment, ArticleCommentDto>()
                .ForMember(dest => dest.user_first_name, opt => opt.MapFrom(src => src.user.first_name))
                .ForMember(dest => dest.user_last_name, opt => opt.MapFrom(src => src.user.last_name));
        }
    }
}
