﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class AddArtistMemberTests : TestsBase
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

            var memberPerson = testPerson;
            var artistMember = new ArtistMember
            {
                Artist = testArtist,
                Member = memberPerson,
                StartedOn = DateTime.Now.AddMonths(-14)
            };

            var task = new AddArtistMember(DbContext);
            var result = task.DoTask(artistMember);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var listArtistMembersTask = new ListArtistMembers(DbContext);
            var listArtistMembersResult = listArtistMembersTask.DoTask(testArtist);

            Assert.IsTrue(listArtistMembersResult.Success);
            Assert.IsNull(listArtistMembersResult.Exception);
            Assert.IsNotNull(listArtistMembersResult.Data);

            var member = listArtistMembersResult.Data.SingleOrDefault(m => m.Id == artistMember.Id);
            Assert.IsNotNull(member);
            Assert.AreEqual(artistMember.StartedOn, member.StartedOn);
            Assert.AreEqual(artistMember.EndedOn, member.EndedOn);
            Assert.AreEqual(artistMember.IsActive, member.IsActive);

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
            var task = new AddArtistMember(EmptyDbContext);
            var result = task.DoTask(new ArtistMember());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
