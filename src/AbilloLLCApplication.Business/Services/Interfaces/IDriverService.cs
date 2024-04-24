using AbilloLLCApplication.Business.DTOs.DriversDTOs;
using AbilloLLCApplication.Business.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vonage.Users;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public interface IDriverService
    {
        List<GetDriverDTO> GetDriversAll();
        Task<GetDriverForOfferCreateDTO> GetDriverForOfferCreateAsync(string userId);
        Task<GetDriverForChangeLocationDTO> GetDriverForChangeLocationAsync(string id);
        Task<GetDriverForChangeDimensionsDTO> GetDriverForChangeDimensionsAsync(string userId);
        Task CreateDriverAsync(PostDriverDTO postDriverDTO);
        Task<GetDriverDTO> GetDriverByIdAsync(Guid id);
        Task<PutDriverDTO> GetDriverForUpdateAsync(Guid id);
        Task UpdateDriverByIdAsync(PutDriverDTO putDriverDTO,Guid Id);
        Task ChangeDriverLocationAsync(ChangeDriverLocationDTO changeDriverLocationDTO,string Id);
        Task ChangeDriverLengthAsync(ChangeDriverLengthDTO changeDriverLengthDTO,string userId);
        Task DeleteDriverByIdAsync(Guid id);


    }
}
