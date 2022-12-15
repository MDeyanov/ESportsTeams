using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESportsTeams.Core.Services;
using ESportsTeams.Core.Models.BindingModels.Tournament;
using NuGet.Packaging;
using Moq;
using ESportsTeams.Infrastructure.Data.Enums;

namespace ESportsTeams.Tests.Services
{
    public class TournamentServiceTest : BaseTest
    {       
        [Test]
        public void EditTournamentAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            var title = "TestTitle";
            var model = new EditTournamentBindingModel()
            {
                Id = 1,
                Title = title,
                Description = "Random mock test description Dota2",
                StartTime = new DateTime(2023, 01, 10),
                Address = new Address
                {
                    City = "Test",
                    Street = "Test",
                    Country = "Test",
                    ZipCode = 123
                }
            };
            var result = service.EditTournamentAsync(model);
            var changedTitle = context.Tournaments.FirstOrDefault(x => x.Title == title);
            Assert.AreEqual(title, changedTitle.Title);
            Assert.AreEqual(1, changedTitle.Id);
        }
        [Test]
        public void GetTournamentByEventIdAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            int expectedCount = 1;
            string title = "mock Dota2 title";
            var result = service.GetTournamentByEventIdAsync(idForTest).Result;
            var firstResult = result.FirstOrDefault();

            Assert.AreEqual(expectedCount, result.Count());
            Assert.AreEqual(idForTest, firstResult.Id);
            Assert.AreEqual(title, firstResult.Title);
        }
        [Test]
        public void GetTournamentByInvalidEventIdAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 100;

            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetTournamentByEventIdAsync(idForTest));
        }

        [Test]
        public void GetTournamentByIdAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            string title = "mock Dota2 title";
            var result = service.GetTournamentByIdAsync(idForTest).Result;



            Assert.AreEqual(idForTest, result.Id);
            Assert.AreEqual(title, result.Title);
        }
        [Test]
        public void GetTournamentDetailsByInvalidIdAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 100;
            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetTournamentDetailsByIdAsync(idForTest));
        }
        [Test]
        public void GetTournamentDetailsByIdAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            string title = context.Tournaments.FirstOrDefault(x => x.Id == idForTest).Title;
            var result = service.GetTournamentDetailsByIdAsync(idForTest).Result;

            Assert.AreEqual(idForTest, result?.Id);
            Assert.AreEqual(title, result?.Title);
        }
        [Test]
        public void ListOfTeamsInTournamentAsyncInvalidTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 100;
            Assert.ThrowsAsync<ArgumentNullException>(() => service.ListOfTeamsInTournamentAsync(idForTest));
        }
        [Test]
        public void ListOfTeamsInTournamentAsyncTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            int countOfTeams = 1;
            var teamName = context.Teams.FirstOrDefault(x => x.Id == idForTest)?.Name;
            var result = service.ListOfTeamsInTournamentAsync(idForTest).Result;
            var firstResult = result.FirstOrDefault();

            Assert.AreEqual(countOfTeams, result.Count());
            Assert.AreEqual(idForTest, firstResult?.Id);
            Assert.AreEqual(teamName, firstResult?.Name);
        }
        [Test]
        public void TeamJoinToTournamentsInvalidIdTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 100;
            string userId = "3";
            Assert.ThrowsAsync<ArgumentNullException>(() => service.TeamJoinToTournaments(userId, idForTest));

        }
        [Test]
        public void TeamJoinToTournamentsInvalidUserIdTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            string userId = "300";
            Assert.ThrowsAsync<ArgumentNullException>(() => service.TeamJoinToTournaments(userId, idForTest));

        }
        [Test]
        public void TeamJoinToTournamentsValidUserButDontOwnAnyTeamTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            string userId = "1";
            Assert.ThrowsAsync<ArgumentNullException>(() => service.TeamJoinToTournaments(userId, idForTest));

        }
        [Test]
        public void TeamJoinToTournamentsValidUserOwnedTeamCategoryInvalidTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int idForTest = 1;
            string userId = "2";
            Assert.ThrowsAsync<ArgumentNullException>(() => service.TeamJoinToTournaments(userId, idForTest));

        }
        [Test]
        public void TeamJoinToTournamentsTest()
        {
            var service = new TournamentService(context, null!, null!, null!);
            int expectedCount = 1;
            int idForTest = 2;
            string userId = context.Users.LastOrDefault().Id;
            var useService = service.TeamJoinToTournaments(userId, idForTest);
            var result = context.Tournaments.FirstOrDefault(x=>x.Id==idForTest)?.TeamTournaments.Count();

            Assert.AreEqual(expectedCount, result);
        }
    }
}
