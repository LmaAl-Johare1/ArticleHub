using AutoMapper;
using Core.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileDto>()
                 .ForMember(
                   dest => dest.followers_count,
                   opt => opt.MapFrom(src => src.user_followers.Count()))
                 .ForMember(
                   dest => dest.followings_count,
                   opt => opt.MapFrom(src => src.user_followings.Count()))
                 .ForMember(
                   dest => dest.articles_count,
                   opt => opt.MapFrom(src => src.user_articles.Count()))
                 .ForMember(
                    dest => dest.is_following,
                    opt => opt.MapFrom((src, dest, destMember, context) => src.user_followers.Select(s => s.User_follower_id).ToList()
                              .Contains((int)context.Items["currentUserId"])));
            CreateMap<UserLoginDto, User>();
            CreateMap<UserForCreationDto, User>();
            CreateMap<UserForUpdateDto, User>();

        }
    }
}
