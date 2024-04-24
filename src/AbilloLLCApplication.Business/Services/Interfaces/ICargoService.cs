using AbilloLLCApplication.Business.DTOs.CargoDTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.PaginationParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public interface ICargoService
    {
        Task<ResponseForPaginationDTO<GetAllCargosDTO>> GetAllCargosAsync(CargoParameters cargoParameters);
        Task<ResponseForPaginationDTO<GetCargosForDriverDTO>> GetNotTakenCargosForDriverAsync(CargoParameters cargoParameters);
        Task<GetCargoDTO> GetCargoByIdAsync(Guid Id);
        Task<GetCargoForOfferPostDTO> GetCargoForOfferPostAsync(Guid Id);
    }
}
