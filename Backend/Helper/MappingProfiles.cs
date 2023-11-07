using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Dtos;
using Backend.Dtos.requests;
using Backend.Models.classes;

namespace Backend.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<User, LoginUserDto>();
            CreateMap<RegisterUserDto, User>();
        }
    }
}