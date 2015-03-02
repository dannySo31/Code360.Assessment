using System.Globalization;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Code360.Assessment.Web.Manager;
using Code360.Assessment.Web.Repository;
using System.Collections.Generic;
using Code360.Assessment.Web.Helpers;



namespace Code360.Assessment.Web.Repository
{
    public interface IUserRepository
    {
      
        
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, ApplicationUserManager Manager);
        Task<string> GenerateEmailConfirmationTokenAsync(string id, ApplicationUserManager Manager);
        Task SendEmailAsync(string userId, string subject, string body, ApplicationUserManager Manager);
        Task<string> GetPhoneNumberAsync(string id, ApplicationUserManager Manager);
        Task<bool> GetTwoFactorEnabledAsync(string id, ApplicationUserManager Manager);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId, ApplicationUserManager Manager);
        Task<bool> TwoFactorBrowserRememberedAsync(string userId, IAuthenticationManager Manager);
        ApplicationUser FindId(string Id, ApplicationUserManager manager);
        Task<List<ApplicationUser>> GetUsers(ApplicationUserManager manager);
        Task<ApplicationUser> FindByIdAsync(string userId, ApplicationUserManager manager);
        Task<IList<string>> GetRolesAsync(string userId, ApplicationUserManager manager);

        IEnumerable<SelectListItem> GetRoles(IList<string> roles,ApplicationRoleManager roleManager);
        Task<IdentityResult> AddToRolesAsync(string userId, ApplicationUserManager manager, params string[] roles);
        Task<IdentityResult> RemoveFromRolesAsync(string userId,ApplicationUserManager manager, params string[] roles);
        
        
       
    }
    public class UserRepository:IUserRepository
    {

       
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, ApplicationUserManager Manager)
        {
            return await Manager.CreateAsync(user,password);
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(string id, ApplicationUserManager Manager)
        {
            return await Manager.GenerateEmailConfirmationTokenAsync(id);
        }


        public async Task SendEmailAsync(string userId, string subject, string body, ApplicationUserManager Manager)
        {
             await Manager.SendEmailAsync(userId, subject, body);
        }





        public async Task<string> GetPhoneNumberAsync(string id, ApplicationUserManager Manager)
        {
            return await Manager.GetPhoneNumberAsync(id);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(string id, ApplicationUserManager Manager)
        {
            return await Manager.GetTwoFactorEnabledAsync(id);
        }


  
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId, ApplicationUserManager Manager)
        {
            return await Manager.GetLoginsAsync(userId);
        }

        public async Task<bool> TwoFactorBrowserRememberedAsync(string userId, IAuthenticationManager Manager)
        {
            return await Manager.TwoFactorBrowserRememberedAsync(userId);
        }

      


        public ApplicationUser FindId(string Id, ApplicationUserManager manager)
        {
            return manager.FindById(Id);
        }
        public async Task<List<ApplicationUser>> GetUsers(ApplicationUserManager manager)
        {
            return await manager.Users.ToListAsync();
        }



        public async Task<ApplicationUser> FindByIdAsync(string userId, ApplicationUserManager manager)
        {
            return await manager.FindByIdAsync(userId);
        }


        public async Task<IList<string>> GetRolesAsync(string userId, ApplicationUserManager manager)
        {
            return await manager.GetRolesAsync(userId);
        }


        public IEnumerable<SelectListItem> GetRoles(IList<string> roles,ApplicationRoleManager roleManager)
        {
            return roleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = roles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });
        }

public async Task<IdentityResult> AddToRolesAsync(string userId, ApplicationUserManager manager, params string[] roles)
{
    return await manager.AddToRolesAsync(userId, roles);
}


public async Task<IdentityResult> RemoveFromRolesAsync(string userId,ApplicationUserManager manager, params string[] roles)
{
    return await manager.RemoveFromRolesAsync(userId, roles);
}
    }
}