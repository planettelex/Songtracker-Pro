using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class UpdateArtistMemberTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var addPersonTask = new AddPerson(DbContext, new FormattingService());
            var testPerson = TestsModel.Person;
            var addPersonResult = addPersonTask.DoTask(testPerson);

            Assert.IsTrue(addPersonResult.Success);
            Assert.IsNull(addPersonResult.Exception);
            Assert.IsNotNull(addPersonResult.Data);

            var memberPerson = testPerson;
            var artistMember = new ArtistMember
            {
                Artist = testArtist,
                Member = memberPerson,
                StartedOn = DateTime.Now.AddMonths(-8)
            };

            var addArtistMemberTask = new AddArtistMember(DbContext);
            var addArtistMemberResult = addArtistMemberTask.DoTask(artistMember);

            Assert.IsTrue(addArtistMemberResult.Success);
            Assert.IsNull(addArtistMemberResult.Exception);
            Assert.IsNotNull(addArtistMemberResult.Data);

            var getArtistMemberTask = new GetArtistMember(DbContext);
            var getArtistMemberResult = getArtistMemberTask.DoTask(artistMember.Id);

            Assert.IsTrue(getArtistMemberResult.Success);
            Assert.IsNull(getArtistMemberResult.Exception);
            Assert.IsNotNull(getArtistMemberResult.Data);

            var member = getArtistMemberResult.Data;
            Assert.IsNotNull(member);
            Assert.AreEqual(artistMember.StartedOn, member.StartedOn);
            Assert.AreEqual(artistMember.EndedOn, member.EndedOn);
            Assert.AreEqual(artistMember.IsActive, member.IsActive);

            member.EndedOn = DateTime.Now.AddDays(-1);

            var task = new UpdateArtistMember(DbContext);
            var result = task.DoTask(member);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getArtistMemberTask = new GetArtistMember(DbContext);
            getArtistMemberResult = getArtistMemberTask.DoTask(artistMember.Id);

            Assert.IsTrue(getArtistMemberResult.Success);
            Assert.IsNull(getArtistMemberResult.Exception);
            Assert.IsNotNull(getArtistMemberResult.Data);

            var updatedMember = getArtistMemberResult.Data;
            Assert.IsNotNull(updatedMember);
            Assert.AreEqual(member.StartedOn, updatedMember.StartedOn);
            Assert.AreEqual(member.EndedOn, updatedMember.EndedOn);
            Assert.AreEqual(false, updatedMember.IsActive);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(memberPerson);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateArtistMember(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
