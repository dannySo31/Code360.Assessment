using Code360.Assessment.Web.Repository;
using IdentitySample.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NBehave.Spec.NUnit;
using Code360.Assessment.Web.Manager;
using IdentitySample.Models;
using System.Collections;
using Microsoft.AspNet.Identity.EntityFramework;
using Code360.Assessment.Web.Helpers;
using Microsoft.AspNet.Identity;

namespace Code360.Assessment.Web.Tests.Controllers
{
    public class when_working_with_the_usersAdmin_controller:Specification
    {
        protected UsersAdminController _controller;
        protected string id = string.Empty;
        protected ApplicationUser user;
        protected Mock<Microsoft.AspNet.Identity.IRoleStore<IdentityRole, string>> _role = new Mock<Microsoft.AspNet.Identity.IRoleStore<IdentityRole, string>>();
      
        protected override void Establish_context()
        {
            base.Establish_context();
            _controller = new UsersAdminController(_repository.Object);
            _controller.UserManager = new ApplicationUserManager(_userStore.Object);
            _role.Object.CreateAsync(new IdentityRole());
            _controller.RoleManager = new ApplicationRoleManager(_role.Object);
         

            user = new ApplicationUser
            {
                Name = "Danny So",
                Email = "y2k301@yahoo.com",
                Picture = new byte[3]

            };
            IList<string> list = new List<string>
                {
                    "Admin"
                };
            _repository.Setup(a => a.FindByIdAsync(It.IsAny<string>(),
                It.IsAny<ApplicationUserManager>())).Returns(Task.FromResult(user));
            _repository.Setup(a => a.GetRolesAsync(It.IsAny<string>(),
                It.IsAny<ApplicationUserManager>()))
                .Returns(Task.FromResult(list));
            id = "wer";
            _repository.Setup(a => a.GetRoles(It.IsAny<IList<string>>(), It.IsAny<ApplicationRoleManager>())).Returns(new List<SelectListItem>());

        }
        public class and_updating_a_user_account:
            when_working_with_the_usersAdmin_controller
        {
            protected EditUserViewModel model = new EditUserViewModel();
            string[] selectedRoles;
            protected override void Establish_context()
            {
                base.Establish_context();
                selectedRoles = new string[]{
                    "admin"
                };
                _repository.Setup(a => a.AddToRolesAsync(It.IsAny<string>(), It.IsAny<ApplicationUserManager>(), It.IsAny<string[]>())).Returns(Task.FromResult(IdentityResult.Success));
                _repository.Setup(a => a.RemoveFromRolesAsync(It.IsAny<string>(), It.IsAny<ApplicationUserManager>(), It.IsAny<string[]>())).Returns(Task.FromResult(IdentityResult.Success));
                model.Image = _httpPostedFileBase.Object;
            }

            [Test]
            public async Task then_viewresult_should_be_index()
            {
                var result = await _controller.Edit(model, selectedRoles) as RedirectToRouteResult;
                var page = result.RouteValues["Action"];
                page.ShouldEqual("Index");
            }

        }
        public class and_viewing_the_index_page:
            when_working_with_the_usersAdmin_controller
        {
            protected override void Establish_context()
            {
                base.Establish_context();
              
            }

            [Test]
            public async Task then_viewResult_should_not_be_null()
            {
                var result = await _controller.Index() as ViewResult;

                result.ShouldNotBeNull();
            }

        }
         public class and_editing_a_user_account_with_valid_id_with_no_picture:
             and_editing_a_user_account_with_valid_id_abstract
         {
             protected override void Establish_context()
             {
                 base.Establish_context();
                 user = new ApplicationUser
                 {
                     Name = "Danny So",
                     Email = "y2k301@yahoo.com",
                     Picture = new byte[0]

                 };
                 _repository.Setup(a => a.FindByIdAsync(It.IsAny<string>(),
               It.IsAny<ApplicationUserManager>())).Returns(Task.FromResult(user));
                 _controller.NoPictureFileLocation = string.Empty;
             }

             [Test]
             public async Task then_model_hasPicture_should_be_false()
             {
                 var result = await BecauseOf();
                 var model = result.Model as EditUserViewModel;
                 model.HasPicture.ShouldBeFalse();
             }
         }
          public abstract class and_editing_a_user_account_with_valid_id_abstract:
              when_working_with_the_usersAdmin_controller
          {
              public async Task<ViewResult> BecauseOf()
              {

                  return await _controller.Edit(id) as ViewResult;
              }
          }
         public class and_editing_a_user_account_with_valid_id:
            and_editing_a_user_account_with_valid_id_abstract
         {
             protected override void Establish_context()
             {
               
                 base.Establish_context();
                
             }
            
             [Test]
             public async Task then_viewModel_should_not_be_null()
             {
                 var result = await BecauseOf();

                 result.ShouldNotBeNull();
             }
             [Test]
             public async Task then_Name_should_not_be_null()
             {
                 var result = await BecauseOf();

                 var model = result.Model as EditUserViewModel;
                 model.Name.ShouldNotBeNull();
             }
             [Test]
             public async Task then_picture_should_not_be_null()
             {
                 var result = await BecauseOf();
                 var model = result.Model as EditUserViewModel;
                 model.Picture.ShouldNotBeNull();
             }
         }
        public class and_editing_a_user_account_with_invalid_id:
            when_working_with_the_usersAdmin_controller
        {
            protected override void Establish_context()
            {
                base.Establish_context();
            }

            [Test]
            public async Task then_page_should_throw_an_exception()
            {
                var result = await _controller.Edit((string)null);
                result.ShouldBeInstanceOfType(typeof(HttpStatusCodeResult));
            }
        }
        public class and_viewing_user_details:
            when_working_with_the_usersAdmin_controller
        {
           
            protected override void Establish_context()
            {
                base.Establish_context();
                
            }
            [Test]
            public async Task then_details_should_not_be_null()
            {
                var result = await _controller.Details(id) as ViewResult;
                result.ShouldNotBeNull();
            }
        }
    }
}
