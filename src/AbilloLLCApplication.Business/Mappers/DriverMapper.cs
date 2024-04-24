using AbilloLLCApplication.Business.DTOs.DriversDTOs;
using AbilloLLCApplication.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Mappers
{
    public class DriverMapper : Profile
    {
        public DriverMapper() 
        {
            CreateMap<Driver,GetDriverForOfferCreateDTO>().ReverseMap();
            CreateMap<Driver, GetDriverDTO>().ForMember(gd=>gd.driverId,x=>x.MapFrom(d=>d.Id)).ReverseMap();
            CreateMap<PostDriverDTO, Driver>().ReverseMap();
            CreateMap<PutDriverDTO, Driver>().ReverseMap();
            CreateMap<ChangeDriverLocationDTO, Driver>().ReverseMap();
            CreateMap<Driver,GetDriverForChangeLocationDTO>().ReverseMap();
            CreateMap<ChangeDriverLengthDTO, Driver>().ReverseMap();
            CreateMap<Driver,GetDriverForChangeDimensionsDTO>().ForMember(gd=>gd.userName,x=>x.MapFrom(d=> d.AppUser  != null ? d.AppUser.UserName : "")).ReverseMap();
        }
    }
}
