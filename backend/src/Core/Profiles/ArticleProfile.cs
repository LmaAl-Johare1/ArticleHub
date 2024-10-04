using AutoMapper;
using Core.Models;
using Data.Entities;
using System.Linq;

namespace Core.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between Article-related DTOs and entities.
    /// </summary>
    public class ArticleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleProfile"/> class.
        /// </summary>
        public ArticleProfile()
        {
            // Mapping from ArticleForCreationDto to Article
            CreateMap<ArticleForCreationDto, Article>()
                .ForMember(dest => dest.image, opt => opt.Ignore());

            // Mapping from Article to ArticleDto
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.tags, opt => opt.MapFrom(src => src.article_tags.Select(at => at.tag.name).ToList())) 
                .ForMember(dest => dest.user_first_name, opt => opt.MapFrom(src => src.user.first_name)) 
                .ForMember(dest => dest.user_last_name, opt => opt.MapFrom(src => src.user.last_name)); 
        }
    }
}
