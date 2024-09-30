using AutoMapper;
using Core.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserLoginDto, User>();
            CreateMap<UserForCreationDto, User>();
        }
    }
}
