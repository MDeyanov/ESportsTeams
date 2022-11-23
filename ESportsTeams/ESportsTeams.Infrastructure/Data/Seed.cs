using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ESportsTeams.Infrastructure.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Tournaments.Any())
                {
                    context.Tournaments.AddRange(new List<Tournament>()
                    {
                        new Tournament()
                        {
                            Title = "BTS Pro Series Season 12: Southeast Asia",
                            Description = "BTS Pro Series Season 12 is an online tournament organized by Beyond The Summit." +
                            " In this season, the tournament will be played in Southeast Asia and Americas region. " +
                            "The Group Stage where 9 teams of each region will be pariticpating in a single round robin format tournament.",
                            Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146442/BTS_Pro_Series_Season_12_q5gp5w.png",
                            StartTime = new DateTime(2023,01,10),
                            Website = "https://www.beyondthesummit.tv/",
                            Facebook = "https://www.facebook.com/BeyondTheSummitTV",
                            PrizePool = 40000.00m,
                            Address = new Address()
                            {
                                Street = "Capital Square 2, 23 Church St",
                                City = "Singapore",
                                Country = "Republic of Singapore",
                                ZipCode = 049481
                            },
                            Event= new Event()
                            {
                                Title = "Dota2",
                                Image = "https://i.pinimg.com/564x/8a/8b/50/8a8b50da2bc4afa933718061fe291520.jpg",
                                Description = "Dota 2 is a multiplayer online battle arena (MOBA)" +
                                " video game in which two teams of five players compete to destroy" +
                                " a large structure defended by the opposing team known as " +
                                "the \"Ancient\" whilst defending their own."
                            }
                        },
                         new Tournament()
                        {
                            Title = "Intel Extreme Masters Rio Major 2022",
                            Description = "The Intel Extreme Masters Season XVII – Rio Major 2022," +
                            " also known as IEM Rio Major 2022 or Rio 2022, is the ongoing eighteenth" +
                            " Counter-Strike: Global Offensive Major Championship.",
                            Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146443/ESL-CSGO_rvxbsm.jpg",
                            StartTime = new DateTime(2022,12,20),
                            Website = "https://pro.eslgaming.com/tour/csgo/rio/#?matchday=3",
                            Facebook = "https://www.facebook.com/iem",
                            PrizePool = 1250000.00m,
                            Address = new Address()
                            {
                                Street = "Av. Embaixador Abelardo Bueno, 3401 - Barra da Tijuca",
                                City = "Rio de Janeiro",
                                Country = "Brazil",
                                ZipCode = 22775
                            },
                            Event= new Event()
                            {
                                Title = "CSGO",
                                Image = "https://i.pinimg.com/564x/17/97/34/1797349b7a00466615057817632d870d.jpg",
                                Description = "The game pits two teams, Terrorists and Counter-Terrorists," +
                                " against each other in different objective-based game modes." +
                                " The most common game modes involve the Terrorists planting a bomb" +
                                " while Counter-Terrorists attempt to stop them, or Counter-Terrorists" +
                                " attempting to rescue hostages that the Terrorists have captured."
                            }
                        },
                         new Tournament()
                        {
                            Title = "LCK Spring 2023",
                            Description = "The LCK 2022 Spring Season is the first split of the second year of Korea's professional League of Legends league under partnership." +
                            " Ten teams compete in a round robin group stage.",
                            Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146442/LCK_2021_woynoe.png",
                             StartTime = new DateTime(2023,02,10),
                            Website = "https://lolesports.com",
                            Facebook = "https://www.facebook.com/officiallck",
                            PrizePool = 300000.00m,
                            Address = new Address()
                            {
                                Street = "33 Jong-ro, Cheongjin-dong, Jongno-gu,",
                                City = "Seoul",
                                Country = "South Korea",
                                ZipCode = 604834
                            },
                            Event= new Event()
                            {
                                Title = "LeagueOfLegends",
                                Image = "https://i.pinimg.com/564x/a2/ea/c1/a2eac1e1644fad2ab6253e7562ebba00.jpg",
                                Description = "League of Legends is one of the world's most popular video games," +
                                " developed by Riot Games." +
                                " It features a team-based competitive game mode based on strategy and outplaying opponents." +
                                " Players work with their team to break the enemy Nexus before the enemy team breaks theirs."

                            }
                        },
                          new Tournament()
                        {
                            Title = "Peacekeeper Elite League Summer 2023",
                            Description = "China's highest level professional league for Peacekeeper Elite," +
                            " a Chinese rebranded version of PUBG Mobile.",
                            Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146442/Peacekeeper_Elite_League_2019_lightmode_erqypu.png",
                             StartTime = new DateTime(2023,02,10),
                            Website = "https://lolesports.com",
                            Facebook = "https://www.facebook.com/officiallck",
                            PrizePool = 300000.00m,
                            Address = new Address()
                            {
                                Street = "3001 Binhai Blvd",
                                City = "Shenzhen",
                                Country = "China",
                                ZipCode = 518064
                            },
                            Event= new Event()
                            {
                                Title = "PUBG",
                                Image = "https://i.pinimg.com/564x/34/28/7f/34287f4927e862d5d86b63d63df3409f.jpg",
                                Description = "PUBG is a player versus player shooter game in which up to one hundred players fight in a battle royale," +
                                " a type of large-scale last man standing deathmatch where players fight to remain the last alive." +
                                " Players can choose to enter the match solo, duo, or with a small team of up to four people."

                            }
                        },
                            new Tournament()
                        {
                            Title = "VALORANT Champions Tour 2023: EMEA League",
                            Description = "China's highest level professional league for Peacekeeper Elite," +
                            " a Chinese rebranded version of PUBG Mobile.",
                            Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1669146443/2022-valorant_mkrlz9.jpg",
                             StartTime = new DateTime(2023,03,26),
                            Website = "https://lolesports.com",
                            Facebook = "https://www.facebook.com/officiallck",
                            PrizePool = 200000.00m,
                            Address = new Address()
                            {
                                Street = "Potsdamer Str. 182",
                                City = "Berlin",
                                Country = "Germany",
                                ZipCode = 10783
                            },
                            Event = new Event()
                            {
                                Title = "VALORANT",
                                Image = "https://i.pinimg.com/564x/39/dd/4d/39dd4da08ecccc0159d79598365e995e.jpg",
                                Description = "Valorant is a team-based first-person hero shooter set in the near future." +
                                " Players play as one of a set of Agents," +
                                " characters based on several countries and cultures around the world." +
                                " In the main game mode, players are assigned to either the attacking or defending team with each team having five players on it."
                            }
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(new List<Team>()
                    {
                        new Team()
                        {
                            Name = "NaPeshoTeama",
                            Description = "Dota2 team for 5v5",
                            Image = "https://upload.wikimedia.org/wikipedia/en/f/f1/Team_Liquid_logo.svg",
                            Category = Category.Dota2,
                            Address = new Address()
                            {
                              Street = "zora 1",
                              City = "Kyustendil",
                              Country = "Bulgaria",
                              ZipCode = 2500
                            },
                            TournamentWin = 0,
                            OwnerId = "1c2bfb08-9038-4cd1-b3de-49efb78f5cd7"                           
                        }                       
                    });
                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "admin",
                        FirstName = "Martin",
                        LastName = "Deyanov",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "zora 1",
                            City = "Kyustendil",
                            Country = "Bulgaria",
                            ZipCode = 2500
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Parola1!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        FirstName = "Pesho",
                        LastName = "Petrov",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 st",
                            City = "Sofia",
                            Country = "Bulgaria",
                            ZipCode = 2500
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Pesho1!");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
