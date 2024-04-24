using AbilloLLCApplication.Business.DTOs.DriversDTOs;
using AbilloLLCApplication.Business.DTOs.OfferDTOs;
using AbilloLLCApplication.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Mappers
{
    public class OfferMapper : Profile
    {
        public OfferMapper() 
        {
            CreateMap<Offer, GetOfferByDriverIdDTO>().ReverseMap();
            CreateMap<Offer,GetOfferByIdForResponseDTO>().ReverseMap();
           CreateMap<PatchOfferDTO,Offer>().ForMember(o=>o.WithPercent,x=>x.Ignore()).ReverseMap();
           CreateMap<PostOfferDTO, Offer>().ForMember(o=>o.WithPercent,x=>x.Ignore()).ReverseMap();
           CreateMap<Offer,GetOfferDTO>().ForPath(go => go.Driver.Fullname, x => x.MapFrom(o => o.Driver.AppUser.Fullname)).ForPath(go=>go.Driver.Username,x=>x.MapFrom(o=>o.Driver.AppUser.UserName)).ForPath(go => go.Driver.DriverId, x => x.MapFrom(o => o.Driver.Id)).ReverseMap();
        }
    }
}
