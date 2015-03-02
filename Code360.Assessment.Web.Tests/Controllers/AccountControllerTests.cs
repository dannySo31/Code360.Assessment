using Code360.Assessment.Web.Manager;
using Code360.Assessment.Web.Repository;
using IdentitySample.Controllers;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NBehave.Spec.NUnit;
using Code360.Assessment.Web.Helpers;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Code360.Assessment.Web.Tests.Controllers
{
    public class when_using_the_account_controller:Specification
    {
        protected AccountController _accountController;
       


        protected override void Establish_context()
        {
            base.Establish_context();
            
        }

        protected override void Because_of()
        {
            base.Because_of();
        }
        public class and_uploading_a_file_with_more_than_8192_kb_limit :
            and_uploading_a_file
        {
            
            protected override void Establish_context()
            {
                base.Establish_context();
            }
            protected override void Because_of()
            {
                _httpPostedFileBase.Setup(a => a.ContentLength).Returns(9000000).Verifiable();
                 

            }

            [Test]

            public async Task then_viewModel_should_not_be_null()
            {
                var result = await _accountController.Register(_viewModel) as ViewResult;
                var model = result.Model as RegisterViewModel;
                model.ShouldNotBeNull();
            }
            [Test]
            public async Task then_result_should_be_an_instance_of_RegisterViewModel()
            {
                var result = await _accountController.Register(_viewModel) as ViewResult;
                result.Model.ShouldBeInstanceOfType<RegisterViewModel>();
            }
        }
         public abstract class and_uploading_a_file :
             when_using_the_account_controller{
                
                
                 
                 
                 protected RegisterViewModel _viewModel;
             protected override void Establish_context()
             {
                 base.Establish_context();
                
                
                 _viewModel = new RegisterViewModel
                 {
                     Image = _httpPostedFileBase.Object,
                     Name = "Danny So",
                     Email = "danny.so@gmail.com",
                     ConfirmPassword = "dso31",
                     Password = "dso31"
                 };
                 _accountController = new AccountController(_repository.Object);
                 _accountController.UserManager = new ApplicationUserManager(_userStore.Object);
               
                 _accountController.ControllerContext = new System.Web.Mvc.ControllerContext(_httpContextBase.Object, new RouteData(), _accountController);
                 _accountController.Url = new UrlHelper(new RequestContext(_httpContextBase.Object, new RouteData()), new RouteCollection());
             }
         }
        public class and_uploading_a_file_with_8192_kb_limit :
            and_uploading_a_file
        {
            
          
           
            protected override void Establish_context()
            {
                base.Establish_context();
              

                
            }

            protected override void Because_of()
            {
                base.Because_of();
                _httpPostedFileBase.Setup(a => a.ContentLength).Returns(8192).Verifiable();
    
            }
            [Test]
            public async Task then_model_should_be_null()
            {
              
                    var result = await _accountController.Register(_viewModel) as ViewResult;
                    var model = result.Model as RegisterViewModel;
                    model.ShouldBeNull();
              
              
            }
            [Test]
            public async Task then_upload_image_size_should_be_smaller_than_8mb()
            {
                var result = await _accountController.Register(_viewModel) as ViewResult;
                _httpPostedFileBase.Object.ContentLength.ShouldBeLessThanOrEqualTo(ValidationHelper.UPLOAD_FILE_SIZE_LIMIT);
            }

            //[Test]
            //public async Task then_saved_image_size_should_be_smaller_than_4mb()
            //{
            //    var result = await _accountController.Register(_viewModel) as ViewResult;
            //    _viewModel.SavedUploadImageSize.ShouldBeLessThanOrEqualTo(ValidationHelper.UPLOAD_SAVED_FILE_SIZE_LIMIT);
            //}
        }
    }
}
