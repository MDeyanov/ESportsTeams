using CloudinaryDotNet.Actions;
using ESportsTeams.Core.Services;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Tests.Services
{
    public class AdminServiceTest : BaseTest
    {
        
        [Test]
        public void GetAllUsers()
        {
            var userManager = GetUserManager();
            var currentUser = context.Users.FirstOrDefault().Id;
            var service = new AdminService(userManager, context);

            var result = service.GetAllUsers(currentUser);
            Assert.AreEqual(2, result.Count);
        }
        private  UserManager<AppUser> GetUserManager()
        {
            var store = new Mock<IUserStore<AppUser>>();
            var mgr = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Setup(x => x.Users).Returns(context.Users);
             

            return mgr.Object;
        }
    }
}
