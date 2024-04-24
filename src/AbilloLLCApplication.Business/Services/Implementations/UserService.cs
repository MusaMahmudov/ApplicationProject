using AbilloLLCApplication.Business.DTOs.UserDTOs;
using AbilloLLCApplication.Business.Exceptions.RoleExceptions;
using AbilloLLCApplication.Business.Exceptions.UserExceptions;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Enums;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vonage.Users;

namespace AbilloLLCApplication.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMessageRepository _messageRepository;
        public UserService(IMessageRepository messageRepository,IHttpContextAccessor accessor,UserManager<AppUser> userManager,IMapper mapper,RoleManager<IdentityRole> roleManager)
        {
            _messageRepository = messageRepository;
            _accessor = accessor;
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;

        }
        public async Task<List<GetUserDTO>> GetAllUserAsync(string? nameFilter)
        {
            var users = _userManager.Users.AsNoTracking().ToList().Where(u=> nameFilter != null ? u.UserName == nameFilter : true);
            var usersDTO = new List<GetUserDTO>();

            foreach (var user in users) 
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDTO = _mapper.Map<GetUserDTO>(user);
                userDTO.Roles = roles.ToList();
                usersDTO.Add(userDTO);
            }



            return usersDTO;

        }
        public async Task<GetUserForUpdateDTO> GetUserForUpdateAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found by Id");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var rolesId = new List<string>();
            foreach (var role in roles)
            {
                var roleId = await _roleManager.Roles.FirstOrDefaultAsync(r=>r.Name == role);
                if(roleId is null)
                {
                    throw new RoleNotFoundByIdException("Role not found");
                }
                rolesId.Add(roleId.Id);
            }

            var userDTO = _mapper.Map<GetUserForUpdateDTO>(user);
            userDTO.RolesId = rolesId;

            return userDTO;
        }
        public async Task<List<GetUserForDriverCreateDTO>> GetUsersForDriverCreateAsync()
        {
            

            var users = _userManager.Users.AsNoTracking().Where(u => u.driver == null).ToList();
            List<GetUserForDriverCreateDTO> usersDTO = new List<GetUserForDriverCreateDTO>(users.Count);
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if(roles.Any(r=>r == Roles.Driver.ToString()))
                {
                    var userDTO = _mapper.Map<GetUserForDriverCreateDTO>(user);
                    usersDTO.Add(userDTO );
                }

            }
            return usersDTO;
        }

        public async Task<List<GetUserForDriverCreateDTO>> GetUsersForDriverUpdateAsync(Guid driverId)
        {
            var users = await _userManager.Users.Where(u=>u.driver == null || u.driver.Id == driverId).ToListAsync();
            List<GetUserForDriverCreateDTO> usersDTO = new List<GetUserForDriverCreateDTO>(users.Count);
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(r => r == Roles.Driver.ToString()))
                {
                    var userDTO = _mapper.Map<GetUserForDriverCreateDTO>(user);
                    usersDTO.Add(userDTO);
                }

            }
            return usersDTO;
        }
        public async Task<GetUserDTO> GetUserAsync(string id)
        {
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(U=>U.Id == id);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found by Id");
            }
            var userDTO = _mapper.Map<GetUserDTO>(user);

            userDTO.Roles = await _userManager.GetRolesAsync(user);
           
            return userDTO;
        }

        public async Task<List<GetAllRolesDTO>> GetAllRolesAsync()
        {
           var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            var rolesDTOs = _mapper.Map<List<GetAllRolesDTO>>(roles);
            return rolesDTOs;
        }
        public async Task CreateUserAsync(PostUserDTO postUserDTO)
        {
            if(postUserDTO.Roles.Count == 0)
            {
                throw new RolesAreRequiredException("Roles required");
            }
            var roles = new List<string>();
            bool hasDriverOrDispatcher = false;

            foreach (var roleId in postUserDTO.Roles)
            {

                var role = await _roleManager.FindByIdAsync(roleId);
                if(role.Name == Roles.Admin.ToString())
                {
                    throw new OnlyOneAdminAllowedException("Only One Admin allowed");
                }
                if(role.Name == Roles.Driver.ToString() || role.Name == Roles.Dispatcher.ToString())
                {
                    if(hasDriverOrDispatcher)
                    {
                        throw new UserCannotBeDispatcherAndDriverException("User has to be driver OR dispatcher,together is forbidden");
                    }

                    hasDriverOrDispatcher = true;
                }

                roles.Add(role.Name);
            }
           
          

            var newUser = _mapper.Map<AppUser>(postUserDTO);
            newUser.LastVisit = DateTime.UtcNow;
            var result = await _userManager.CreateAsync(newUser,postUserDTO.Password);

            if(!result.Succeeded) 
            {
                throw new CreateUserException(result.Errors);
            }
           await _userManager.AddToRolesAsync(newUser,roles);




            
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                throw new UserNotFoundByIdException("User not found by Id");
            }
            if((await _userManager.GetRolesAsync(user)).Any(r=>r == Roles.Admin.ToString()))
            {
                throw new AdminCannotBeDeletedException("Admin cannot be deleted");
            }
            await _userManager.DeleteAsync(user);
            
        }

        public async Task UpdateUserByIdAsync(PutUserDTO putUserDTO, string id)
        {
            if (putUserDTO.RolesId.Count == 0)
            {
                throw new RolesAreRequiredException("Roles required");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new UserNotFoundByIdException("User not found by Id");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var newRoles = new List<string>(putUserDTO.RolesId.Count);
            var hasDriverOrDispatcher = false;

            foreach (var roleId in putUserDTO.RolesId)
            {
                var newRole = await _roleManager.FindByIdAsync(roleId);

                if (newRole is null)
                {
                    throw new RoleNotFoundByIdException("Role not found");
                }
                if (hasDriverOrDispatcher && (newRole.Name == Roles.Dispatcher.ToString() || newRole.Name == Roles.Driver.ToString()))
                {
                    throw new UserCannotBeDispatcherAndDriverException("User cannot be dispatcher and driver at the same time");
                }
                if(newRole.Name == Roles.Dispatcher.ToString() || newRole.Name == Roles.Driver.ToString())
                {
                    hasDriverOrDispatcher = true;

                }
                newRoles.Add(newRole.Name);
            }


            if (roles.Contains(Roles.Admin.ToString()) && !newRoles.Contains(Roles.Admin.ToString()))
            {
                throw new Admin_sRoleCannotBeChangedException("Admin can't loose ADMIN ROLE");
            }
            if(!roles.Contains(Roles.Admin.ToString()) && newRoles.Contains(Roles.Admin.ToString()))
            {
               throw new OnlyOneAdminAllowedException("Only one admin allowed");
            }

            var rolesToRemove = roles.Except(putUserDTO.RolesId);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRolesAsync(user, newRoles);

            if (putUserDTO.Password is not null && putUserDTO.Password.Length > 0)
            {
                if (putUserDTO.ConfirmPassword != putUserDTO.Password)
                {
                    throw new ConfirmPasswordIncorrectException("Confirm password incorrect");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var identityResult = await _userManager.ResetPasswordAsync(user, token, putUserDTO.Password);
                if (!identityResult.Succeeded)
                {
                    throw new ResetPasswordException(identityResult.Errors);
                }
            }

            user = _mapper.Map(putUserDTO, user);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new UpdateUserException(updateResult.Errors);
            }


        }

        public async Task ChangePasswordAsync(string id, ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null) 
            {
                throw new UserNotFoundByIdException("User not found");
            }
            if(changePasswordDTO.OldPassword == changePasswordDTO.NewPassword)
            {
                throw new OldPasswordEqualToNewPasswordException("Old password equal to new password");
            }
            if(!await _userManager.CheckPasswordAsync(user, changePasswordDTO.OldPassword))
            {
              throw new OldPasswordIncorrectException("Old password incorrect");
            }
          var result = await _userManager.ChangePasswordAsync(user,changePasswordDTO.OldPassword,changePasswordDTO.NewPassword);
            if (!result.Succeeded)
            {
                throw new ChangePasswordException(result.Errors);
            }

        }

        public async Task<List<GetUserForChatDTO>> GetUsersForChatAsync()
        {
            var userName = _accessor.HttpContext.User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if(user is null )
            {
                throw new UserNotFoundByIdException("User not found");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(Roles.Admin.ToString()))
            {
                var users = await _userManager.GetUsersInRoleAsync(Roles.Driver.ToString());

                var usersDTO = new List<GetUserForChatDTO>(users.Count);
                foreach (var us in users)
                {
                   
                    var messages = _messageRepository.GetFiltered(m => (m.ReceiverId == us.Id && m.SenderId == user.Id) || (m.ReceiverId == user.Id && m.SenderId == us.Id), false,"Receiver","Sender").ToList();
                    var userDTO  = _mapper.Map<GetUserForChatDTO>(us);
                    if(messages.Count() != 0)
                    {
                          
                         userDTO.lastMessage = messages[0].Content;
                        userDTO.isReadLastMessage = messages[0].IsRead;
                        userDTO.senderId = messages[0].SenderId;


                    }

                    usersDTO.Add(userDTO);

                };



                return usersDTO;
            }
            else
            {
                var users = await _userManager.GetUsersInRoleAsync(Roles.Admin.ToString());
                var usersDTO = new List<GetUserForChatDTO>(users.Count);
                foreach(var us in users)
                {
                    var messages = _messageRepository.GetFiltered(m => (m.ReceiverId == us.Id && m.SenderId == user.Id) || (m.ReceiverId == user.Id && m.SenderId == us.Id), false, "Receiver", "Sender").ToList();
                    var userDTO = _mapper.Map<GetUserForChatDTO>(us);
                    if (messages.Count() != 0)
                    {

                        userDTO.lastMessage = messages[0].Content;
                        userDTO.isReadLastMessage = messages[0].IsRead;
                        userDTO.senderId = messages[0].SenderId;
                    }

                    usersDTO.Add(userDTO);
                }

                return usersDTO;

            }

        }
    }
}
