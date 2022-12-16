using ESportsTeams.Core.Services;
using ESportsTeams.Infrastructure.Data.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Tests.Services
{
    public class UserServiceTest : BaseTest
    {
        [Test]
        public void CurrentUserTeamsHaveCategoryTest()
        {
            var service = new UserService(context);
            var userId = context.Users.First().Id;
            var category = Category.Dota2;
            var otherUserId = context.Users.Last().Id;

            var result = service.CurrentUserTeamsHaveCategory(userId, category).Result;
            var resultToReturnFalse = service.CurrentUserTeamsHaveCategory(otherUserId, category).Result;

            Assert.IsTrue(result);
            Assert.IsFalse(resultToReturnFalse);
        }

        [Test]
        public void FindUserByIdInvaldIdAsyncTest()
        {
            var service = new UserService(context);
            var userId = "InvalidId";

            Assert.ThrowsAsync<ArgumentNullException>(() => service.FindUserByIdAsync(userId));

        }

        [Test]
        public void FindUserByIdAsyncTest()
        {
            var service = new UserService(context);
            var userId = context.Users.First().Id;

            var result = service.FindUserByIdAsync(userId).Result;

            Assert.AreEqual(userId, result.Id);
        }

        [Test]
        public void GetUserByIDInvalidIdTest()
        {
            var service = new UserService(context);
            var userId = "InvalidId";

            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetUserByID(userId));
        }
        [Test]
        public void GetUserByIDTest()
        {
            var service = new UserService(context);
            var userId = context.Users.First().Id;
            var currentUser = context.Users.First(x => x.Id == userId);
            var result = service.GetUserByID(userId).Result;

            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(currentUser.UserName, result.Username);
        }
        [Test]
        public void GetUserOwnedTeamsAsyncTest()
        {
            var service = new UserService(context);
            var userId = context.Users.Last().Id;
            var category = Category.Dota2;
            var result = service.GetUserOwnedTeamsAsync(userId, category, 0, 6).Result;

            Assert.AreEqual(1, result.Count());
        }
    }
}
