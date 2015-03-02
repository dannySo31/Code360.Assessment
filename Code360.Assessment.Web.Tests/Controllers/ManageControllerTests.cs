using IdentitySample.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NBehave.Spec.NUnit;
using Moq;
using Code360.Assessment.Web.Repository;
using Code360.Assessment.Web.Manager;
using IdentitySample.Models;
using System.Web.Routing;
using Microsoft.Owin.Security;

namespace Code360.Assessment.Web.Tests.Controllers
{
   
    public class when_working_with_the_managecontroller:Specification
    {
        protected ManageController _controller;
       
        protected override void Establish_context()
        {
            base.Establish_context();
        }
        protected override void Because_of()
        {
            base.Because_of();
        }

        public class and_viewing_the_details_page :
            when_working_with_the_managecontroller
        {
            protected override void Establish_context()
            {
                base.Establish_context();
                _controller = new ManageController(_repository.Object);
                _controller.UserManager = new ApplicationUserManager(_userStore.Object);
                _controller.AuthenticationManager = _authenticationmanager.Object;
                _controller.UserName = "Test";
                _repository.Setup(a => a.FindId(It.IsAny<string>(), It.IsAny<ApplicationUserManager>())).Returns((ApplicationUser)null);
                _controller.ControllerContext = new System.Web.Mvc.ControllerContext(_httpContextBase.Object, new RouteData(), _controller);
                _controller.Url = new UrlHelper(new RequestContext(_httpContextBase.Object, new RouteData()), new RouteCollection());
               
            }

            [Test]
            public async Task then_viewResult_should_not_be_null()
            {
                var result = await _controller.Index(ManageController.ManageMessageId.AddPhoneSuccess) as ViewResult;

                result.ShouldNotBeNull();
            }
        }
    }
}
