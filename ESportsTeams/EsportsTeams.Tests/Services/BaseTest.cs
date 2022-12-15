using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using ESportsTeams.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ESportsTeams.Tests.Services
{
    public class BaseTest
    {
        protected ApplicationDbContext context;
        protected List<AppUser>? users;
        protected List<Event>? events;
        protected List<Team>? teams;
        protected List<Tournament>? tournaments;
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
            teams = new List<Team>
            {
               new Team
               {
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
                   IsBanned= false,
               },
               new Team
               {
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
                   IsBanned= false,
               }
            };
            tournaments = new List<Tournament>
            {
               new Tournament
                {
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
            events = new List<Event>
            {
                new Event
                {
                  Title = "Dota2",
                  Image = "Test",
                  Description = "TestTestTestTestTestTest",
                  IsDeleted= false,
                },
                new Event
                {
                  Title = "CSGO",
                  Image = "Test",
                  Description = "TestTestTestTestTestTest",
                  IsDeleted= false,
                }
            };

           
            foreach (var team in teams)
            {
                team.OwnerId = users[2].Id;
            }

            context.Tournaments.AddRangeAsync(tournaments);
            context.Events.AddRangeAsync(events);
            context.Users.AddRangeAsync(users);
            context.Teams.AddRangeAsync(teams);
            context.SaveChangesAsync();
            foreach (var team in teams)
            {
                team.AppUsers.Add(users.FirstOrDefault(x => x.Id == users[2].Id));
            }
            var teamTournament = tournaments.FirstOrDefault(x => x.Id == 1);
            teamTournament?.TeamTournaments.Add(new TeamTournament()
            {
                TeamId = teams[0].Id,
                Team = teams[0],
                TournamentId = tournaments[0].Id,
                Tournament = tournaments[0],
            });
            context.SaveChangesAsync();
        }

        [TearDown]
        public void ClearDB()
        {
            context.Database.EnsureDeleted();
            users = null;
            events = null;
            teams = null;
            tournaments = null;
            teamsTournament = null;

        }

    }
}
