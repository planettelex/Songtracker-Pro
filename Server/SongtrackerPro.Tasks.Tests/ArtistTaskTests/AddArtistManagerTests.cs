using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class AddArtistManagerTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var testArtist = TestsModel.Artist;
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);
            Assert.IsNotNull(addArtistResult.Data);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var addPersonTask = new AddPerson(DbContext, new FormattingService());
            var testPerson = TestsModel.Person;
            var addPersonResult = addPersonTask.DoTask(testPerson);

            Assert.IsTrue(addPersonResult.Success);
            Assert.IsNull(addPersonResult.Exception);
            Assert.IsNotNull(addPersonResult.Data);

            var managerPerson = testPerson;
            var artistManager = new ArtistManager
            {
                Artist = testArtist,
                Manager = managerPerson,
                StartedOn = DateTime.Now.AddMonths(-14)
            };

            var task = new AddArtistManager(DbContext);
            var result = task.DoTask(artistManager);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var listArtistManagersTask = new ListArtistManagers(DbContext);
            var listArtistManagersResult = listArtistManagersTask.DoTask(testArtist);

            Assert.IsTrue(listArtistManagersResult.Success);
            Assert.IsNull(listArtistManagersResult.Exception);
            Assert.IsNotNull(listArtistManagersResult.Data);

            var manager = listArtistManagersResult.Data.SingleOrDefault(m => m.Id == artistManager.Id);
            Assert.IsNotNull(manager);
            Assert.AreEqual(artistManager.StartedOn, manager.StartedOn);
            Assert.AreEqual(artistManager.EndedOn, manager.EndedOn);
            Assert.AreEqual(artistManager.IsActive, manager.IsActive);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(managerPerson);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddArtistManager(EmptyDbContext);
            var result = task.DoTask(new ArtistManager());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
