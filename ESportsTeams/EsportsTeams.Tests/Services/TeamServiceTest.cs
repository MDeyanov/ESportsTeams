using ESportsTeams.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESportsTeams.Core.Services;
using ESportsTeams.Infrastructure.Data.Enums;
using System.Threading.Tasks;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http;
using Moq;
using ESportsTeams.Core.Interfaces;
using CloudinaryDotNet.Actions;
using ESportsTeams.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
using static System.Formats.Asn1.AsnWriter;

namespace ESportsTeams.Tests.Services
{
    public class TeamServiceTest : BaseTest
    {      
        [Test]
        public async Task AddTeamAsyncWithValidTeam()
        {
            var service = new TeamService(context, null!, null!);
            var expectedId = 3;
            var team = new AddTeamBindingModel
            {
                Name = "Test",
                Description = "Test",
                Address = new Address
                {
                    City = "Test",
                    Country = "Test",
                    ZipCode = 123,
                    Street = "Test",
                },
                Image = null,
                Category = Category.Dota2

            };
            var userId = context.Users.FirstOrDefault().Id;
            await service.AddTeamAsync(team, userId);

            var currnetTeam = context.Teams.FirstOrDefault(x => x.Id == expectedId);
            var teamsCount = context.Teams.Count();
            Assert.AreEqual(team.Name, currnetTeam?.Name);
            Assert.AreEqual(3, teamsCount);
        }

        [Test]
        public void AddTeamAsyncWithInvalidUser()
        {
            var service = new TeamService(context, null!, null!);
            var team = new AddTeamBindingModel
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                Address = new Address
                {
                    City = "Test",
                    Country = "Test",
                    ZipCode = 123,
                    Street = "Test",
                },
                Image = null,
                Category = Category.Dota2
            };
            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddTeamAsync(team, "ewapoiueyhwa"));
        }
        [Test]
        public void AddTeamAsyncUserCategoryCheck()
        {
            var service = new TeamService(context, null!, null!);
            var teamBindingModel = new AddTeamBindingModel
            {
                Id = 5,
                Name = "Test",
                Description = "Test",
                Address = new Address
                {
                    City = "Test",
                    Country = "Test",
                    ZipCode = 123,
                    Street = "Test",
                },
                Image = null,
                Category = Category.Dota2
            };
            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddTeamAsync(teamBindingModel, "3"));
        }

        [Test]
        public void GetCountAsync()
        {
            var areEqual = context.Teams.Count();
            var service = new TeamService(context, null!, null!);

            var result = service.GetCountAsync().Result;
            Assert.AreEqual(result, areEqual);
        }

        [Test]
        public void GetCountInCategoryAsync()
        {
            var areEqual = context.Teams.Where(x => x.Category == Category.Dota2).Count();
            var service = new TeamService(context, null!, null!);

            var result = service.GetCountByCategoryAsync(Category.Dota2).Result;
            Assert.AreEqual(result, areEqual);
        }

        [Test]
        public void GetCountOfUserOwnedTeamsAsync()
        {
            var areEqual = context.Teams.Where(x => x.OwnerId == "3").Count();
            var service = new TeamService(context, null!, null!);

            var result = service.GetOwnedTeamCountAsync("3").Result;
            Assert.AreEqual(result, areEqual);
        }

        [Test]
        public async Task GetTeamDetails()
        {
            var service = new TeamService(context, null!, null!);
            var userId = context.Users.FirstOrDefault().Id;

            var result = service.GetTeamDetailsByIdAsync(2, userId).Result;
            var contextdrbr = context.Teams.FirstOrDefault(x => x.Id == 2);
            Assert.AreEqual(result?.Id, 2);
        }

        [Test]
        public async Task GetTeamById()
        {
            var service = new TeamService(context, null!, null!);
            var result = context.Teams.FirstOrDefault(x => x.Id == 1);

            var secRes = service.GetTeamByIdAsync(11);

            var areEqual = context.Teams.FirstOrDefault(x => x.Id == 1);

            Assert.AreEqual(result?.Id, areEqual?.Id);
            Assert.AreEqual(result.Name, areEqual.Name);
        }
        [Test]
        public void GetAllTeams()
        {
            var service = new TeamService(context, null!, null!);
            var result = service.GetAllTeamsAsync().Result;
            var toCheck = context.Teams.Count();
       

            Assert.AreEqual(result.Count(), toCheck);
        }
        [Test]
        public void TeamExistsTest()
        {
            var service = new TeamService(context, null!, null!);
            var isExists = service.TeamExistsAsync("CSGOTeam").Result;

            Assert.AreEqual(isExists, true);
        }
        [Test]
        public void TeamExistsNullNameTest()
        {
            var service = new TeamService(context, null!, null!);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.TeamExistsAsync("Error"));
        }
        [Test]
        public void EditTeamAsync()
        {
            var service = new TeamService(context, null!, null!);
            var model = new EditTeamBindingModel()
            {
                Id= 1,
                Name = "Dota2TeamTEST",
                Description = "Random mock test description Dota2",
                Category = Category.Dota2,
                AddressId = 1,
                Address = new Address
                {
                    City = "Test",
                    Street = "Test",
                    Country = "Test",
                    ZipCode = 1,
                },
            };
            var teamName = "Dota2TeamTEST";
            var result = service.EditTeamAsync(model);

            var changedName = context.Teams.FirstOrDefault(x=>x.Id == 1);
            Assert.AreEqual(changedName?.Name, teamName);
        }
        [Test]
        public void JoinTeamTest()
        {
            var service = new TeamService(context, null!, null!);
            //user with id "1" join to team with id 1!
            var test = service.JoinTeam("1", 1);
            var result = context.Teams.FirstOrDefault(x=>x.Id==1);
            var reqCount = result.Requests.Count();

            Assert.AreEqual(1, reqCount);
        }        
        [Test]
        public void ApproveUserTest()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(users[0]);

            var service = new TeamService(context, null!, userService.Object);
            var makeReq = service.JoinTeam("1", 1);
            var reqId = context.Teams.FirstOrDefault(x => x.Id == 1).Requests[0].Id;
            var test = service.ApproveUser(reqId);
            var result = context.Teams.FirstOrDefault(x => x.Id == 1).AppUsers.Count();

            Assert.AreEqual(1, result);
        }
        [Test]
        public void DeclineUserInvalidTest()
        {
            var service = new TeamService(context, null!, null!);
            Assert.ThrowsAsync<ArgumentNullException>(() => service.DeclineUser(123));
        }
        [Test]
        public void DeclineUserTest()
        {
            var service = new TeamService(context, null!, null!);
            var makeReq = service.JoinTeam("1", 1);
            var reqId = context.Requests.FirstOrDefault().Id;
            var test = service.DeclineUser(reqId);
            var isDecline = context.Requests.FirstOrDefault(x=>x.Id== reqId).Status;
            Assert.AreEqual(RequestStatus.Declined, isDecline);
        }
    }
}
