using AutoMapper;
using Core.Models;
using Data.Entities;
using System;
using System.Linq;

namespace Core.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="User"/> and related DTOs.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// Configures mappings for user-related data transfer objects (DTOs).
        /// </summary>
        public UserProfile()
        {
            // Mapping configuration from User entity to UserProfileDto.
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
                    opt => opt.MapFrom((src, dest, destMember, context) =>
                        src.user_followers
                        .Select(s => s.User_follower_id)
                        .Contains((int)context.Items["currentUserId"])));

            // Mapping configuration for UserLoginDto to User entity.
            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password));

            // Mapping configuration for UserForCreationDto to User entity.
            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.created, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.updated, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping configuration for UserForUpdateDto to User entity.
            CreateMap<UserForUpdateDto, User>()
                .ForMember(dest => dest.updated, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
