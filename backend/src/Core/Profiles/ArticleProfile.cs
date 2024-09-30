using AutoMapper;
using Core.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
     
        }
    }
}
