using AbilloLLCApplication.Business.DTOs.UserDTOs;
using AbilloLLCApplication.Core.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<AppUser, GetUserDTO>().ReverseMap();
            CreateMap<PostUserDTO, AppUser>().ReverseMap();
            CreateMap<PutUserDTO,AppUser>().ReverseMap();
            CreateMap<AppUser, GetUserForDriverDTO>().ReverseMap();
            CreateMap<AppUser,GetUserForDriverCreateDTO>().ReverseMap();
            CreateMap<AppUser,GetUserForUpdateDTO>().ReverseMap();
            CreateMap<IdentityRole,GetAllRolesDTO>().ReverseMap();
            CreateMap<AppUser,GetUserForChatDTO>().ReverseMap();
        }
    }
}
