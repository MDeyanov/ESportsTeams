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
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;

namespace ESportsTeams.Tests.Services
{
    public class EventServiceTest : BaseTest
    {       
        [Test]
        public void GetEventByIdAsyncTest()
        {
            int eventId =2;
            var service = new EventService(context, null!, null!);

            var result = service.GetEventByIdAsync(eventId).Result;

            Assert.AreEqual(eventId, result?.Id);
            Assert.AreEqual("CSGO", result?.Title);
        }
        [Test]
        public void GetEventByTitleAsyncTest()
        {
            string eventTitle = "CSGO";
            var service = new EventService(context, null!, null!);

            var result = service.GetEventByTitleAsync(eventTitle).Result;

            Assert.AreEqual("CSGO", result?.Title);
        }
        [Test]
        public void GetEventByTitleAsyncIfNullTest()
        {
            string eventTitle = "WillBeNull";
            var service = new EventService(context, null!, null!);

            var result = service.GetEventByTitleAsync(eventTitle).Result;

            Assert.IsNull(result);
        }

        [Test]
        public void DeleteTest()
        {
            var service = new EventService(context, null!, null!);
            int eventId = 1;
            var result = service.Delete(eventId);

            Assert.AreEqual(eventId, result);
        }
        [Test]
        public void DeleteIfNullTest()
        {
            var service = new EventService(context, null!, null!);
            int eventId = 100;
            var result = service.Delete(eventId);
            var expect = -1;
            
            Assert.AreEqual(expect, result);
        }

        [Test]
        public void ReverseIsDeletedTest()
        {
            var service = new EventService(context, null!, null!);
            int expectedEventId = 1;
            var result123 = service.ReverseIsDeleted(expectedEventId);

            Assert.AreEqual(expectedEventId, result123);

        }
        [Test]
        public void ReverseIsDeletedIfNullTest()
        {
            var service = new EventService(context, null!, null!);
            int eventId = 100;
            var result = service.ReverseIsDeleted(eventId);
            var expect = -1;

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void EditEventAsyncTest()
        {
             var service = new EventService(context, null!, null!);
             var title = "Dota2TitleEditTest";
             var model = new EditEventBindingModel()
             {
                 Id = 1,
                 Title = title,
                 Description = "TestTestTestTestTestTest",
             };
             var result = service.EditEventAsync(model);
       
             var eventChangedTitle = context.Events.FirstOrDefault(x => x.Title == title);
             Assert.AreEqual(title, eventChangedTitle?.Title);
        }
        [Test]
        public void EditEventAsyncInvalidModelIdTest()
        {
             var service = new EventService(context, null!, null!);
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
            var service = new EventService(context, null!, null!);
            var invalidId = 100;
            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetEventDetailsByIdAsync(invalidId));
        }
        [Test]
        public void GetEventDetailsByIdAsyncTest()
        {
            var service = new EventService(context, null!, null!);
            var eventId = 1;
            var result = service.GetEventDetailsByIdAsync(eventId).Result;
            var eventInContext = context.Events.FirstOrDefault(x=>x.Id== eventId);

            Assert.AreEqual(eventId, result.Id);
            Assert.AreEqual(eventInContext?.Title, result.Title);
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
            var service = new EventService(context, null!, null!);
            var result = service.GetAllAsync().Result;

            var countOfEvents = context.Events.Where(x=>!x.IsDeleted).Count();

            Assert.AreEqual(countOfEvents, result.Count());
        }
    }
}

 