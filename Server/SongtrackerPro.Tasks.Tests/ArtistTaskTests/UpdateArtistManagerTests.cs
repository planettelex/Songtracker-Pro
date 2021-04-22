using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class UpdateArtistManagerTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext);
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var addPersonTask = new AddPerson(DbContext);
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
                StartedOn = DateTime.Now.AddMonths(-8)
            };

            var addArtistManagerTask = new AddArtistManager(DbContext);
            var addArtistManagerResult = addArtistManagerTask.DoTask(artistManager);

            Assert.IsTrue(addArtistManagerResult.Success);
            Assert.IsNull(addArtistManagerResult.Exception);
            Assert.IsNotNull(addArtistManagerResult.Data);

            var getArtistManagerTask = new GetArtistManager(DbContext);
            var getArtistManagerResult = getArtistManagerTask.DoTask(artistManager.Id);

            Assert.IsTrue(getArtistManagerResult.Success);
            Assert.IsNull(getArtistManagerResult.Exception);
            Assert.IsNotNull(getArtistManagerResult.Data);

            var manager = getArtistManagerResult.Data;
            Assert.IsNotNull(manager);
            Assert.AreEqual(artistManager.StartedOn, manager.StartedOn);
            Assert.AreEqual(artistManager.EndedOn, manager.EndedOn);
            Assert.AreEqual(artistManager.IsActive, manager.IsActive);

            manager.EndedOn = DateTime.Now.AddDays(-1);

            var task = new UpdateArtistManager(DbContext);
            var result = task.DoTask(manager);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getArtistManagerTask = new GetArtistManager(DbContext);
            getArtistManagerResult = getArtistManagerTask.DoTask(artistManager.Id);

            Assert.IsTrue(getArtistManagerResult.Success);
            Assert.IsNull(getArtistManagerResult.Exception);
            Assert.IsNotNull(getArtistManagerResult.Data);

            var updatedManager = getArtistManagerResult.Data;
            Assert.IsNotNull(updatedManager);
            Assert.AreEqual(manager.StartedOn, updatedManager.StartedOn);
            Assert.AreEqual(manager.EndedOn, updatedManager.EndedOn);
            Assert.AreEqual(false, updatedManager.IsActive);

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
            var task = new UpdateArtistManager(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
