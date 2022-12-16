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
        public void GetAllUsersTest()
        {
            var userManager = GetUserManager();
            var currentUser = context.Users.FirstOrDefault().Id;
            var service = new AdminService(userManager, context);

            var result = service.GetAllUsers(currentUser);
            Assert.AreEqual(2, result.Count);
        }
        [Test]
        public void GetUserTest()
        {
            var userManager = GetUserManager();
            var idForSearch = context.Users.LastOrDefault().Id;
            var expectedName = context.Users.LastOrDefault().UserName;
            var service = new AdminService(userManager, context);

            var result = service.GetUser(idForSearch);
            Assert.AreEqual(idForSearch, result.UserId);
            Assert.AreEqual(expectedName, result.Username);
        }
        [Test]
        public void GetUserInvalidIdTest()
        {
            var userManager = GetUserManager();
            var idForSearch = "InvalidId";
            var service = new AdminService(userManager, context);
          
            var result = service.GetUser(idForSearch);
            Assert.IsNull(result);
        }
        [Test]
        public void BanUserTest()
        {
            var userManager = GetUserManager();
            var banUserId = context.Users.LastOrDefault().Id;
            var service = new AdminService(userManager, context);
            var currentUser = context.Users.FirstOrDefault(x=>x.Id== banUserId);

            var result = service.BanUser(banUserId);

            Assert.AreEqual(banUserId, result);
            Assert.IsTrue(currentUser.IsBanned);
            Assert.IsTrue(currentUser.OwnedTeams.FirstOrDefault().IsBanned);
            Assert.AreEqual(1, currentUser.OwnedTeams.LastOrDefault().AppUsers.Count());
        }
        [Test]
        public void BanUserInvalidIdTest()
        {
            var userManager = GetUserManager();
            var banUserId = "InvalidId";
            var service = new AdminService(userManager, context);
            var currentUser = "NoSuchUser";

            var result = service.BanUser(banUserId);

            Assert.AreEqual(currentUser, result);
        }
        [Test]
        public void UnBanUserInvalidIdTest()
        {
            var userManager = GetUserManager();
            var UnBanUserId = "InvalidId";
            var service = new AdminService(userManager, context);
            var currentUser = "NoSuchUser";

            var result = service.BanUser(UnBanUserId);

            Assert.AreEqual(currentUser, result);
        }
        [Test]
        public void UnBanUserTest()
        {
            var userManager = GetUserManager();
            var service = new AdminService(userManager, context);
            var currentUserId = context.Users.FirstOrDefault().Id;
            var checkUser = context.Users.FirstOrDefault(x=>x.Id==currentUserId);
            var firstBanUser = service.BanUser(currentUserId);

            var unBanUser = service.UnbanUser(currentUserId);

            Assert.AreEqual(currentUserId, unBanUser);
            Assert.IsFalse(checkUser.IsBanned);
            Assert.IsFalse(checkUser.OwnedTeams.Any(x=>x.IsBanned==false));
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
