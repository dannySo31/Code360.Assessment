using System.Globalization;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Code360.Assessment.Web.Manager;
using Code360.Assessment.Web.Repository;
using Code360.Assessment.Web.Helpers;
using ImageResizer;
using Code360.Assessment.Web.Models;

namespace Code360.Assessment.Web.Controllers
{
    public abstract class BaseController:Controller
    {
        #region Constructors
        public BaseController()
        {

        }
        public BaseController(IUserRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("IUserRepository");
            UserRepository = repository;
          
        }
        public BaseController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public BaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        } 
        #endregion
        #region Public Properties
        private IUserRepository _repository;
        public IUserRepository UserRepository
        {
            get
            {
                return _repository ?? new UserRepository();
            }
            set
            {
                _repository = value;
            }
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }
        private string _noPictureFileLocation;
        public string NoPictureFileLocation
        {
            get
            {
                return _noPictureFileLocation ?? Server.MapPath(Url.Content(ValidationHelper.NO_IMAGE));
            }
            set{
                _noPictureFileLocation = value;
            }
        }
        #endregion

        internal bool UploadSizeLimitReached(UserBase user)
        {
            bool state = false;
            if (user.Image.ContentLength > ValidationHelper.UPLOAD_FILE_SIZE_LIMIT)
            {
                ModelState.AddModelError("UploadFileLimitReached", "Upload file should not exceed 8MB");
                state = true;

            }
            return state;
        }
    }
}