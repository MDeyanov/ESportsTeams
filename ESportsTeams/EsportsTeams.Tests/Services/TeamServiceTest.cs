﻿using ESportsTeams.Infrastructure.Data;
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

namespace ESportsTeams.Tests.Services
{
    public class TeamServiceTest
    {
        protected ApplicationDbContext context;
        protected List<AppUser>? users;
        protected List<Team>? teams;
        protected PhotoService? photoService;
        protected List<IdentityRole>? roles;

        [SetUp]
        public void InitializeDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseInMemoryDatabase("ApplicationInMemoryDB").Options;

            context = new ApplicationDbContext(options);
            users = new List<AppUser>
            {
                new AppUser
                {
                    Id = "1",
                    UserName = "FirstUser",
                    Email = "first@abv.bg",
                    FirstName = "First1",
                    LastName = "Last1",
                    Dota2MMR = 2000,
                    NormalizedEmail = "FIRST@ABV.BG",
                    NormalizedUserName = "FIRSTUSER",
                },
                new AppUser
                {
                    Id = "2",
                    UserName = "SecondUser",
                    Email = "second@abv.bg",
                    FirstName = "First2",
                    LastName = "Last2",
                    Dota2MMR = 1500,
                    NormalizedEmail = "SECOND@ABV.BG",
                    NormalizedUserName = "SECONDUSER",
                },
                new AppUser
                {
                    Id = "3",
                    UserName = "ThirdUser",
                    Email = "third@abv.bg",
                    FirstName = "First3",
                    LastName = "Last3",
                    Dota2MMR = 1700,
                    NormalizedEmail = "THIRD@ABV.BG",
                    NormalizedUserName = "THIRDUSER",

                }
            };
            teams = new List<Team>
            {
               new Team
               {
                   Id= 1,
                   Name = "Dota2Team",
                   Description = "Random mock test description Dota2",
                   Category = Category.Dota2,
                   AddressId= 1,
                   Address = new Address
                   {
                       City = "Test",
                       Street= "Test",
                       Country= "Test",
                       ZipCode=1,
                   },
                   TournamentWin =0,
                   OwnerId = "3",
                   IsBanned= false,
               },
               new Team
               {
                   Id= 2,
                   Name = "CSGOTeam",
                   Description = "Random mock test description CSGO",
                   Category = Category.CSGO,
                   AddressId= 2,
                   Address = new Address
                   {
                       City = "Test1",
                       Street= "Test1",
                       Country= "Test1",
                       ZipCode=1,
                   },
                   TournamentWin =0,
                   OwnerId = "3",
                   IsBanned= false,
               }
            };
            ////roles = new List<IdentityRole>
            ////{
            ////    new IdentityRole
            ////    {

            ////    }
            ////};
            //context.Roles.AddRangeAsync()
            context.Users.AddRangeAsync(users);
            context.Teams.AddRangeAsync(teams);
            context.SaveChangesAsync();

        }
        [Test]
        public async Task AddTeamAsyncWithValidTeam()
        {
            var service = new TeamService(context, null, null);
            var team = new AddTeamBindingModel
            {
                Id = 3,
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
            await service.AddTeamAsync(team, "1");

            var currnetTeam = context.Teams.FirstOrDefault(x => x.Id == team.Id);
            Assert.AreEqual(team.Id, currnetTeam?.Id);
            Assert.AreEqual(team.Name, currnetTeam?.Name);
        }
        [Test]
        public void AddTeamAsyncWithInvalidUser()
        {
            var service = new TeamService(context, null, null);
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
            var service = new TeamService(context, null, null);
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
            var service = new TeamService(context, null, null);

            var result = service.GetCountAsync().Result;
            Assert.AreEqual(result, areEqual);
        }
        [Test]
        public void GetCountInCategoryAsync()
        {
            var areEqual = context.Teams.Where(x => x.Category == Category.Dota2).Count();
            var service = new TeamService(context, null, null);

            var result = service.GetCountByCategoryAsync(Category.Dota2).Result;
            Assert.AreEqual(result, areEqual);
        }
        [Test]
        public void GetCountOfUserOwnedTeamsAsync()
        {
            var areEqual = context.Teams.Where(x => x.OwnerId == "3").Count();
            var service = new TeamService(context, null, null);

            var result = service.GetOwnedTeamCountAsync("3").Result;
            Assert.AreEqual(result, areEqual);
        }
        [Test]
        public async Task GetTeamDetails()
        {
            var service = new TeamService(context, null, null);

            var result = service.GetTeamDetailsByIdAsync(1, "1");


        }

        [Test]
        public async Task GetTeamById()
        {
            var service = new TeamService(context, null, null);
            var result = context.Teams.FirstOrDefault(x => x.Id == 1);

            var secRes = service.GetTeamByIdAsync(1);

            var areEqual = context.Teams.FirstOrDefault(x => x.Id == 1);

            Assert.AreEqual(result.Id, areEqual.Id);
            Assert.AreEqual(result.Name, areEqual.Name);
        }
        [Test]
        public void GetAllTeams()
        {
            var service = new TeamService(context, null, null);
            var user = context.Users.FirstOrDefault(x => x.Id == "3");
            var result = service.GetAllTeamsAsync().Result;
            var toCheck = result.FirstOrDefault(x => x.Id == 1);
            var first = new GetTeamsViewModel
            {
                Id = 1,
                Name = "Dota2Team",
                Description = "Random mock test description Dota2",
                Category = Category.Dota2,
                Address = new Address
                {
                    Street = "test"
                },
                TournamentWin = 0,
                Owner = user,
                IsBanned = false,
            };

            Assert.AreEqual(toCheck.Id, first.Id);
            Assert.AreEqual(toCheck.Name, first.Name);
        }
    }
}
