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
    public class TournamentServiceTest
    {
        protected ApplicationDbContext context;
        protected List<AppUser>? users;
        protected List<Tournament>? tournaments;
        protected List<Event>? events;
        protected List<Team>? teams;
        protected List<TeamTournament>? teamsTournament;

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
                    CSGOMMR=1500,
                    PUBGMMR=1500,
                    LeagueOfLegendsMMR=1500,
                    VALORANTMMR=1500,
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
                    CSGOMMR=1500,
                    PUBGMMR=1500,
                    LeagueOfLegendsMMR=1500,
                    VALORANTMMR=1500,
                    NormalizedEmail = "THIRD@ABV.BG",
                    NormalizedUserName = "THIRDUSER",

                }
            };
            tournaments = new List<Tournament>
            {
               new Tournament
                {
                    Id = 1,
                    Title = "mock Dota2 title",
                    Description = "Random mock test description Dota2",
                    Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146442/BTS_Pro_Series_Season_12_q5gp5w.png",
                    StartTime = new DateTime(2023,01,10),
                    Website = "https://www.beyondthesummit.tv/",
                    Facebook = "https://www.facebook.com/BeyondTheSummitTV",
                    PrizePool = 40000.00m,
                    AddressId = 1,
                    EventId = 1                    
                },
                new Tournament
                {
                    Id = 2,
                    Title = "mock CSGO title",
                    Description = "Random mock test description Dota2",
                    Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146443/ESL-CSGO_rvxbsm.jpg",
                    StartTime = new DateTime(2022,12,20),
                    Website = "https://pro.eslgaming.com/tour/csgo/rio/#?matchday=3",
                    Facebook = "https://www.facebook.com/iem",
                    PrizePool = 1250000.00m,
                    AddressId = 2,
                    EventId = 2
                }
            };
            events = new List<Event>
            {
                new Event
                {
                  Id = 1,
                  Title = "Dota2",
                  Image = "Test",
                  Description = "TestTestTestTestTestTest",
                  IsDeleted= false,
                },
                new Event
                {
                  Id = 2,
                  Title = "CSGO",
                  Image = "Test",
                  Description = "TestTestTestTestTestTest",
                  IsDeleted= false,
                },

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
                   OwnerId = "2",
                   IsBanned= false,
               }
            };
            teamsTournament = new List<TeamTournament>
            {
                new TeamTournament
                {
                    TeamId = teams[0].Id,
                    Team = teams[0],
                    TournamentId = tournaments[0].Id,
                    Tournament = tournaments[0],
                }

            };

            var teamTournament = tournaments.FirstOrDefault(x => x.Id == 1);
            teamTournament?.TeamTournaments.Add(new TeamTournament()
            {
                TeamId = teams[0].Id,
                Team = teams[0],
                TournamentId = tournaments[0].Id,
                Tournament = tournaments[0],
            });
              
          

            context.Teams.AddRangeAsync(teams);
            context.Tournaments.AddRangeAsync(tournaments);
            context.Users.AddRangeAsync(users);
            context.Events.AddRangeAsync(events);
            context.SaveChangesAsync();
        }
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
            string userId = "2";
            var useService = service.TeamJoinToTournaments(userId, idForTest);
            var result = context.Tournaments.FirstOrDefault(x=>x.Id==idForTest)?.TeamTournaments.Count();

            Assert.AreEqual(expectedCount, result);
        }
    }
}
