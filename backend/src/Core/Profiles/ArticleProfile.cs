using AutoMapper;
using Core.Models;
using Data.Entities;
using System.Linq;

namespace Core.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleCardDto>()
                .ForMember(
                  dest => dest.likes_count,
                  opt => opt.MapFrom(src => src.article_likes.Count()));

            CreateMap<ArticleForCreationDto, Article>()
                .ForMember(dest => dest.image, opt => opt.Ignore());

            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.tags, opt => opt.MapFrom(src => src.article_tags.Select(at => at.tag.name).ToList()))
                .ForMember(dest => dest.user_first_name, opt => opt.MapFrom(src => src.user.first_name)) 
                .ForMember(dest => dest.user_last_name, opt => opt.MapFrom(src => src.user.last_name)); 
        }
    }
    
}
