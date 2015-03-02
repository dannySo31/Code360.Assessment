using Code360.Assessment.Web.Manager;
using Code360.Assessment.Web.Repository;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Code360.Assessment.Web.Tests
{
    [TestFixture]
    public class Specification:SpecBase
    {
        protected Mock<HttpRequestBase> _httpRequestBase = new Mock<HttpRequestBase>();
        protected Mock<HttpContextBase> _httpContextBase = new Mock<HttpContextBase>();
        protected Mock<IUserRepository> _repository = new Mock<IUserRepository>();
        protected Mock<IUserStore<ApplicationUser>> _userStore = new Mock<IUserStore<ApplicationUser>>();
        protected Mock<IAuthenticationManager> _authenticationmanager = new Mock<IAuthenticationManager>();
        protected Mock<HttpPostedFileBase> _httpPostedFileBase = new Mock<HttpPostedFileBase>();
        protected Mock<HttpFileCollectionBase> _fileCollectionBase = new Mock<HttpFileCollectionBase>();


        protected override void Establish_context()
        {
            _httpContextBase.Setup(a => a.Request).Returns(_httpRequestBase.Object);
            _httpContextBase.Setup(a => a.Request.Url).Returns(new Uri("http://localhost/"));
           
            _repository.SetupAllProperties();
            _userStore.SetupAllProperties();
            _repository.Setup(a => a.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<ApplicationUserManager>())).Returns(Task.FromResult(IdentityResult.Success));
            _repository.Setup(a => a.GenerateEmailConfirmationTokenAsync(It.IsAny<string>(), It.IsAny<ApplicationUserManager>())).Returns(Task.FromResult("ok"));
            _repository.Setup(a => a.SendEmailAsync(It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<ApplicationUserManager>()
             )).Returns(Task.FromResult("ok"));

            _repository.Setup(a => a.GetPhoneNumberAsync(It.IsAny<string>(),
                It.IsAny<ApplicationUserManager>()
                )).Returns(Task.FromResult("ok"));
            _repository.Setup(a => a.GetTwoFactorEnabledAsync(It.IsAny<string>(),
                It.IsAny<ApplicationUserManager>()
                )).Returns(Task.FromResult(true));
            IList<UserLoginInfo> list = new List<UserLoginInfo>
            {
                new UserLoginInfo(null,null)
            };
            var fakeFileKeys = new List<string>() { "file" };


            _httpPostedFileBase.SetupAllProperties();

            _authenticationmanager.SetupAllProperties();
            _fileCollectionBase.SetupAllProperties();
            _httpRequestBase.Setup(a => a.Files).Returns(_fileCollectionBase.Object);

            _fileCollectionBase.Setup(a => a.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());

            _fileCollectionBase.Setup(a => a["file"]).Returns(_httpPostedFileBase.Object);
            _repository.Setup(a => a.GetLoginsAsync(It.IsAny<string>(), It.IsAny<ApplicationUserManager>())).Returns(Task.FromResult(list));

            
            _repository.Setup(a => a.TwoFactorBrowserRememberedAsync(It.IsAny<string>(),
                It.IsAny<IAuthenticationManager>()
                )).Returns(Task.FromResult(true));


        }
    }
}
