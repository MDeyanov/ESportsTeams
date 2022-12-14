using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
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
using Moq;
using ESportsTeams.Core.Models.BindingModels.Event;
using ESportsTeams.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using static System.Net.WebRequestMethods;

namespace ESportsTeams.Tests.Services
{
    public class EventServiceTest
    {
        protected ApplicationDbContext context;
        protected List<AppUser>? users;
        protected List<Event>? events;
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
                }
            };
            roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id="1",
                    Name = UserRoles.Admin
                },
                new IdentityRole
                {
                    Id="2",
                    Name = UserRoles.User
                }
            };
            //var user = users.FirstOrDefault(x => x.Id == "1");
            //var roleManager = GetRoleManager();
            //var userManager = GetUserManager();

           

            //var result = userManager.AddToRoleAsync(user, UserRoles.Admin);

            context.Roles.AddRangeAsync(roles);
            context.Events.AddRangeAsync(events);
            context.Users.AddRangeAsync(users);
            context.SaveChangesAsync();
            


        }

        [Test]
        public void GetEventByIdAsyncTest()
        {
            int eventId = 1;
            var service = new EventService(context, null, null);

            var result = service.GetEventByIdAsync(eventId).Result;

            Assert.AreEqual(eventId, result.Id);
            Assert.AreEqual("Dota2", result.Title);
        }
        [Test]
        public void GetEventByTitleAsyncTest()
        {
            string eventTitle = "Dota2";
            var service = new EventService(context, null, null);

            var result = service.GetEventByTitleAsync(eventTitle).Result;

            Assert.AreEqual("Dota2", result.Title);
        }
        [Test]
        public void GetEventByTitleAsyncIfNullTest()
        {
            string eventTitle = "WillBeNull";
            var service = new EventService(context, null, null);

            var result = service.GetEventByTitleAsync(eventTitle).Result;

            Assert.IsNull(result);
        }

        [Test]
        public void DeleteTest()
        {
            var service = new EventService(context, null, null);
            int eventId = 1;
            var result = service.Delete(eventId);

            Assert.AreEqual(eventId, result);
        }
        [Test]
        public void DeleteIfNullTest()
        {
            var service = new EventService(context, null, null);
            int eventId = 100;
            var result = service.Delete(eventId);
            var expect = -1;
            
            Assert.AreEqual(expect, result);
        }

        [Test]
        public void ReverseIsDeletedTest()
        {
            var service = new EventService(context, null, null);
            int eventId = 1;
            var result = service.ReverseIsDeleted(eventId);

            Assert.AreEqual(eventId, result);
        }
        [Test]
        public void ReverseIsDeletedIfNullTest()
        {
            var service = new EventService(context, null, null);
            int eventId = 100;
            var result = service.ReverseIsDeleted(eventId);
            var expect = -1;

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void EditEventAsyncTest()
        {
             var service = new EventService(context, null, null);
             var title = "Dota2TitleEditTest";
             var model = new EditEventBindingModel()
             {
                 Id = 1,
                 Title = title,
                 Description = "TestTestTestTestTestTest",
             };
             var result = service.EditEventAsync(model);
       
             var eventChangedTitle = context.Events.FirstOrDefault(x => x.Title == title);
             Assert.AreEqual(title, eventChangedTitle.Title);
        }
        [Test]
        public void EditEventAsyncInvalidModelIdTest()
        {
             var service = new EventService(context, null, null);
             var title = "Dota2";
             var model = new EditEventBindingModel()
             {
                 Id = 100,
                 Title = title,
                 Description = "TestTestTestTestTestTest",
             };
             Assert.ThrowsAsync<ArgumentNullException>(() => service.EditEventAsync(model));
        }
        [Test]
        public void GetEventDetailsByInvalidIdAsyncTest()
        {
            var service = new EventService(context, null, null);
            var invalidId = 100;
            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetEventDetailsByIdAsync(invalidId));
        }
        [Test]
        public void GetEventDetailsByIdAsyncTest()
        {
            var service = new EventService(context, null, null);
            var eventId = 1;
            var result = service.GetEventDetailsByIdAsync(eventId).Result;
            var eventInContext = context.Events.FirstOrDefault(x=>x.Id== eventId);

            Assert.AreEqual(eventId, result.Id);
            Assert.AreEqual(eventInContext.Title, result.Title);
        }
        //[Test]
        //public void AddEventAsyncTest()
        //{

        //    var photoService = new Mock<IPhotoService>();
        //    photoService.Setup(_=>_.AddPhotoAsync(It.IsAny<IFormFile>()))
        //        .Returns(new ImageUploadResult { Url = new Uri("www.tralal.com") });
        //    var service = new EventService(context, null, null);
        //    var model = new AddEventBindingModel()
        //    {
        //        Id = 1,
        //    };
        //}
        [Test]
        public void GetAllAsyncTest()
        {
            var service = new EventService(context, null, null);
            var result = service.GetAllAsync().Result;

            var countOfEvents = context.Events.Count();

            Assert.AreEqual(countOfEvents, result.Count());
        }
    }
}

 