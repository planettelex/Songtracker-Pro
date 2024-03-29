﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class AddRecordLabelTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddRecordLabel(DbContext, new FormattingService());
            var testRecordLabel = TestsModel.RecordLabel;
            var result = task.DoTask(testRecordLabel);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var recordLabelId = result.Data;
            Assert.IsNotNull(recordLabelId);
            Assert.IsTrue(recordLabelId > 0);

            var getRecordLabelTask = new GetRecordLabel(DbContext);
            var recordLabel = getRecordLabelTask.DoTask(recordLabelId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(recordLabel);
            Assert.AreEqual(testRecordLabel.Name, recordLabel.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testRecordLabel.TaxId), recordLabel.TaxId);
            Assert.AreEqual(testRecordLabel.Email, recordLabel.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testRecordLabel.Phone), recordLabel.Phone);
            Assert.IsNotNull(testRecordLabel.Address);
            Assert.AreEqual(testRecordLabel.Address.Street, recordLabel.Address.Street);
            Assert.AreEqual(testRecordLabel.Address.City, recordLabel.Address.City);
            Assert.AreEqual(testRecordLabel.Address.Region, recordLabel.Address.Region);
            Assert.AreEqual(testRecordLabel.Address.PostalCode, recordLabel.Address.PostalCode);
            Assert.IsNotNull(testRecordLabel.Address.Country);
            Assert.AreEqual(testRecordLabel.Address.Country.Name, recordLabel.Address.Country.Name);
            Assert.AreEqual(testRecordLabel.Address.Country.IsoCode, recordLabel.Address.Country.IsoCode);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeResult = removeRecordLabelTask.DoTask(recordLabel);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddRecordLabel(EmptyDbContext, new FormattingService());
            var result = task.DoTask(new RecordLabel());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
