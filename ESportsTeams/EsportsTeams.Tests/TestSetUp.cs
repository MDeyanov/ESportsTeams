using CloudinaryDotNet.Actions;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using ESportsTeams.Infrastructure.Data.Enums;
using Moq;

namespace EsportsTeams.Tests
{
    public class TestSetUp
    {
        protected const string MOCK_USER_ID = "mockUserId";
        protected const string MOCK_REFRESH_TOKEN = "mockRefreshToken";
        protected const string MOCK_EMAIL_ADDRESS = "mochMail@mail.com";
        protected const string MOCK_USERNAME = "mochUsername";
        protected const string MOCK_TEAM_ID = "mockTeamId";
        protected const string MOCK_TOURNAMENT_ID = "mockTournamentId";
        protected const string MOCK_EVENT_ID = "mockEventId";
        protected const string MOCK_REQUEST_ID = "mockReqId";

        protected List<AppUser> users;
        protected List<Team> teams;
        protected List<Tournament> tournaments;
        protected List<Request> requests;
        protected List<Address> addresses;
        protected List<Event> events;
        protected List<TeamTournament> teamTournaments;

        protected ApplicationDbContext context;

        public async Task InitializeDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("ApplicationInMemoryDB").Options;

            context = new ApplicationDbContext(options);

            if (context != null)
            {
                context.Database.EnsureDeleted();
            }
            var passwordHasher = new PasswordHasher<AppUser>();

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

            foreach (var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "Parola!1");
            }

            events = new List<Event>
            {
                new Event
                {
                    Id=1,
                    Title = "Dota2",
                    Description = "Random mock test description Dota2",
                    Image = "https://i.pinimg.com/564x/8a/8b/50/8a8b50da2bc4afa933718061fe291520.jpg",
                    IsDeleted= false,
                },
                new Event
                {
                    Id=1,
                    Title = "CSGO",
                    Description = "Random mock test description CSGO",
                    Image = "https://i.pinimg.com/564x/17/97/34/1797349b7a00466615057817632d870d.jpg",
                    IsDeleted= false,
                }
            };
            addresses = new List<Address>
            {
                new Address
                {
                    Id= 1,
                    Street = "Adress 1 street",
                    City = "Kyustendil",
                    Country = "Bulgaria",
                    ZipCode = 2500
                },
                new Address
                {
                    Id= 2,
                    Street = "Adress 2 street",
                    City = "Sofia",
                    Country = "Bulgaria",
                    ZipCode = 1000
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
                    TournamentWin =0,
                    OwnerId = "2",
                    IsBanned= false,
                },
                new Team
                {
                    Id= 2,
                    Name = "CSGOTeam",
                    Description = "Random mock test description CSGO",
                    Category = Category.CSGO,
                    AddressId= 2,
                    TournamentWin =0,
                    OwnerId = "3",
                    IsBanned= false,
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

            teamTournaments = new List<TeamTournament>
            {
                new TeamTournament
                {
                    TeamId= 1,
                    TournamentId = 1,
                },
                new TeamTournament
                {
                    TeamId= 2,
                    TournamentId = 2,
                }
            };

            requests = new List<Request>
            {
                new Request
                {
                    Id= 1,
                    Status = RequestStatus.Pending,
                    TeamId= 1,
                    RequesterId="3",

                },
                new Request
                {
                    Id= 2,
                    Status = RequestStatus.Pending,
                    TeamId= 2,
                    RequesterId="2",

                }
            };

            await context.AddRangeAsync(addresses);
            await context.AddRangeAsync(users);
            await context.AddRangeAsync(events);
            await context.AddRangeAsync(tournaments);
            await context.AddRangeAsync(teams);
            await context.AddRangeAsync(teamTournaments);
            await context.AddRangeAsync(requests);
        }

        public UserManager<AppUser> GetUserManager()
        {
            var store = new Mock<IUserStore<AppUser>>();
            var mgr = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<AppUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<AppUser>());

            mgr.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<AppUser, string>((x, y) => users.Add(x));
            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            mgr.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(true);
            mgr.Setup(x => x.FindByEmailAsync(MOCK_EMAIL_ADDRESS))
                .ReturnsAsync(users.FirstOrDefault(y => y.Email == MOCK_EMAIL_ADDRESS));
            //mgr.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
            //    .ReturnsAsync(roles.Select(x => x.Name).ToList());
            mgr.Setup(x => x.AddToRolesAsync(It.IsAny<AppUser>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Success);          
            mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(users[0]);
            mgr.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.SetEmailAsync(users[0], "first@abv.bg"))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<AppUser, string>((x, y) => x.Email = y);       
            mgr.Setup(x => x.SetUserNameAsync(users[0], "FirstUser"))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<AppUser, string>((x, y) => x.UserName = y);

            return mgr.Object;
        }
    }
}
