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

namespace ESportsTeams.Tests.Services
{
    public class TournamentServiceTest
    {
        protected ApplicationDbContext context;
        protected List<AppUser>? users;
        protected List<Tournament>? tournaments;

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
             
            context.Tournaments.AddRangeAsync(tournaments);
            context.Users.AddRangeAsync(users);
            context.SaveChangesAsync();
        }
        [Test]
        public void EditTournamentAsyncTest()
        {
            var service = new TournamentService(context, null, null,null);
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
            var changedTitle = context.Tournaments.FirstOrDefault(x=>x.Title== title);
            Assert.AreEqual(title, changedTitle.Title);
            Assert.AreEqual(1, changedTitle.Id);
        }
    }
}
