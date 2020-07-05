using AutoMapper;
using OrkadWeb.Models;
using OrkadWeb.Services.DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Services.DTO.Users
{
    public static class UserMappings
    {
        public static TextValue ToTextValue(this User user) => new TextValue
        {
            Value = user.Id.ToString(),
            Text = user.Username
        };
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, TextValue>()
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Id));
        }
    }
}
