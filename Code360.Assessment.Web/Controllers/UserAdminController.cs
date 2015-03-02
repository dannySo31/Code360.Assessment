using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Code360.Assessment.Web.Manager;
using Code360.Assessment.Web.Repository;
using Code360.Assessment.Web.Controllers;
using Code360.Assessment.Web.Helpers;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : BaseController
    {
        public UsersAdminController()
        {
        }
        public UsersAdminController(IUserRepository repository):base(repository)
        {
           

        }
        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager):
            base(userManager,roleManager)
        {
          
        }

       
        

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await UserRepository.GetUsers(UserManager));
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await base.UserRepository.FindByIdAsync(id, UserManager);
            if (!user.HasPicture)
                user.Picture = ImageHelper.ConvertImageToByteArray(NoPictureFileLocation);
            ViewBag.RoleNames = await base.UserRepository.GetRolesAsync(user.Id, UserManager);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserRepository.FindByIdAsync(id, UserManager);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (!user.HasPicture)
                user.Picture = ImageHelper.ConvertImageToByteArray(NoPictureFileLocation);
           

            var userRoles = await UserRepository.GetRolesAsync(user.Id,UserManager);
            var viewModel=new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name= user.Name,
                Picture= user.Picture,
                
                RolesList = UserRepository.GetRoles(userRoles,RoleManager)
            };
            //recreate role

            return View(viewModel);
        }

        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,Name,Picture,Image")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                if (base.UploadSizeLimitReached(editUser))
                    return View(editUser);
                var user = await UserRepository.FindByIdAsync(editUser.Id,UserManager);
                if (user == null)
                {
                    return HttpNotFound();
                }

                PopulateUser(editUser, user);

                var userRoles = await UserRepository.GetRolesAsync(user.Id, UserManager);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserRepository.AddToRolesAsync(user.Id,UserManager, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserRepository.RemoveFromRolesAsync(user.Id,UserManager, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
           
            return await Edit(editUser.Id);
        }

        private static void PopulateUser(EditUserViewModel editUser, ApplicationUser user)
        {
            var picture = new byte[0];
            //check if file is greater than the max save size;
            if (editUser.Image != null)
            {
                picture = ImageHelper.ConvertUploadedFile(editUser.Image.InputStream, ValidationHelper.UPLOAD_SAVED_FILE_SIZE_LIMIT);
            }
            else
                picture = user.Picture;
            user.Name = editUser.Name;
            user.UserName = editUser.Email;
            user.Email = editUser.Email;
            user.Picture = picture;
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
      
    }
}
