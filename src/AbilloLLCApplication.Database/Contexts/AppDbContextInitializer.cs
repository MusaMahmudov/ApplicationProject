using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Database.Contexts
{
    public class AppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AppDbContextInitializer(AppDbContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task InitializerAsync()
        {
           await _context.Database.MigrateAsync();
        }

        public async Task addRolesAsync()
        {
           foreach(var role in Enum.GetValues(typeof(Roles)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role.ToString()));

            }

            AppUser admin = new AppUser() 
            {
             Fullname = "Admin",
             UserName = "Admin",
             Email = "example@gmail.com",
             EmailConfirmed = true,
             ConnectionId = null
            };
           
            await _userManager.CreateAsync(admin,"emxa!");
           await _userManager.AddToRoleAsync(admin,Roles.Admin.ToString());



        }

    }
}
