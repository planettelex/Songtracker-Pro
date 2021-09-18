using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.LegalEntityTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class UpdateLegalEntityTests : TestsBase
    {
        public void UpdateLegalEntityModel(LegalEntity legalEntity)
        {
            var stamp = DateTime.Now.Ticks;
            legalEntity.Name = "Update " + stamp;
            legalEntity.TradeName = "Trade name " + stamp;
            legalEntity.TaxId = stamp.ToString();
            legalEntity.Email = $"test@update{stamp}.com";
            legalEntity.Address = TestsModel.Address;
            legalEntity.Services = new List<Service>();

            var listServicesTask = new ListServices(DbContext);
            var allServices = listServicesTask.DoTask(ServiceType.LegalEntity).Data;

            const int maxServiceCount = 5;
            var numberOfServices = new Random().Next(1, maxServiceCount);

            for (var i = 0; i < numberOfServices; i++)
            {
                var randomIndex = new Random().Next(allServices.Count - 1);
                legalEntity.Services.Add(allServices[randomIndex]);
            }
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testLegalEntity = TestsModel.LegalEntity;
            var addLegalEntityTask = new AddLegalEntity(DbContext, new FormattingService());
            var addLegalEntityResult = addLegalEntityTask.DoTask(testLegalEntity);

            Assert.IsTrue(addLegalEntityResult.Success);
            Assert.IsNull(addLegalEntityResult.Exception);

            var task = new UpdateLegalEntity(DbContext, new FormattingService());
            var toUpdate = testLegalEntity;
            UpdateLegalEntityModel(toUpdate);
            var result = task.DoTask(toUpdate);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getLegalEntityTask = new GetLegalEntity(DbContext);
            var legalEntity = getLegalEntityTask.DoTask(toUpdate.Id)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(legalEntity);
            Assert.AreEqual(toUpdate.Name, legalEntity.Name);
            Assert.AreEqual(toUpdate.TradeName, legalEntity.TradeName);
            Assert.AreEqual(formattingService.FormatTaxId(toUpdate.TaxId), legalEntity.TaxId);
            Assert.AreEqual(toUpdate.Email, legalEntity.Email);
            Assert.AreEqual(toUpdate.Address.Street, legalEntity.Address.Street);
            Assert.AreEqual(toUpdate.Address.City, legalEntity.Address.City);
            Assert.AreEqual(toUpdate.Address.Region, legalEntity.Address.Region);
            Assert.AreEqual(toUpdate.Address.PostalCode, legalEntity.Address.PostalCode);
            Assert.AreEqual(toUpdate.Address.Country.Name, legalEntity.Address.Country.Name);

            var removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            var removeLegalEntityResult = removeLegalEntityTask.DoTask(legalEntity);

            Assert.IsTrue(removeLegalEntityResult.Success);
            Assert.IsNull(removeLegalEntityResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateLegalEntity(EmptyDbContext, new FormattingService());
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
