using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.LegalEntityTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class AddLegalEntityTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddLegalEntity(DbContext, new FormattingService());
            var testLegalEntity = TestsModel.LegalEntity;
            var result = task.DoTask(testLegalEntity);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var legalEntityId = result.Data;
            Assert.IsNotNull(legalEntityId);
            Assert.IsTrue(legalEntityId > 0);

            var getLegalEntityTask = new GetLegalEntity(DbContext);
            var legalEntity = getLegalEntityTask.DoTask(legalEntityId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(legalEntity);
            Assert.AreEqual(testLegalEntity.Name, legalEntity.Name);
            Assert.AreEqual(testLegalEntity.TradeName, legalEntity.TradeName);
            Assert.AreEqual(formattingService.FormatTaxId(testLegalEntity.TaxId), legalEntity.TaxId);
            Assert.AreEqual(testLegalEntity.Email, legalEntity.Email);
            Assert.IsNotNull(legalEntity.Address);
            Assert.AreEqual(testLegalEntity.Address.Street, legalEntity.Address.Street);
            Assert.AreEqual(testLegalEntity.Address.City, legalEntity.Address.City);
            Assert.AreEqual(testLegalEntity.Address.Region, legalEntity.Address.Region);
            Assert.AreEqual(testLegalEntity.Address.PostalCode, legalEntity.Address.PostalCode);
            Assert.IsNotNull(legalEntity.Address.Country);
            Assert.AreEqual(testLegalEntity.Address.Country.Name, legalEntity.Address.Country.Name);
            Assert.AreEqual(testLegalEntity.Address.Country.IsoCode, legalEntity.Address.Country.IsoCode);

            var removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            var removeResult = removeLegalEntityTask.DoTask(legalEntity);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddLegalEntity(EmptyDbContext, new FormattingService());
            var result = task.DoTask(new LegalEntity());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
