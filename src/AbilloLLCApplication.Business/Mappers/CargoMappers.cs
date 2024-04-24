using AbilloLLCApplication.Business.DTOs.CargoDTOs;
using AbilloLLCApplication.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Mappers
{
    public class CargoMappers : Profile
    {
        public CargoMappers() 
        {
            CreateMap<Cargo, GetCargosForDriverDTO>().ForMember(gc=>gc.Notes,x=>x.Ignore()).ReverseMap();
            CreateMap<Cargo, GetAllCargosDTO>().ForMember(gc=>gc.NewOffers,x=>x.MapFrom(c=>c.Offers.Where(o=>o.Accepted == null).Count())).ReverseMap();
         CreateMap<Cargo,GetCargoDTO>().ReverseMap();
            CreateMap<Cargo,GetCargoForOfferDTO>().ReverseMap();
            CreateMap<Cargo,GetCargoForOfferPatchDTO>().ReverseMap();
            CreateMap<Cargo, GetCargoForOfferPostDTO>().ReverseMap();
        }

    }
}
