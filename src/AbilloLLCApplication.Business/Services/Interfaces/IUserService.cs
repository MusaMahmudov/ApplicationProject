using AbilloLLCApplication.Business.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<GetUserDTO>> GetAllUserAsync(string? nameFilter);
        Task<GetUserDTO> GetUserAsync(string id);
        Task<GetUserForUpdateDTO> GetUserForUpdateAsync(string id);
        Task<List<GetUserForDriverCreateDTO>> GetUsersForDriverCreateAsync();
        Task<List<GetUserForDriverCreateDTO>> GetUsersForDriverUpdateAsync(Guid driverId);
        Task CreateUserAsync(PostUserDTO postUserDTO);
        Task DeleteUserAsync(string id);
        Task UpdateUserByIdAsync(PutUserDTO putUserDTO,string id);
        Task ChangePasswordAsync(string id,ChangePasswordDTO changePasswordDTO);
        Task<List<GetAllRolesDTO>> GetAllRolesAsync();
        Task<List<GetUserForChatDTO>> GetUsersForChatAsync();

    }
}
