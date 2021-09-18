using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.LegalEntityTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class GetLegalEntityTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addLegalEntityTask = new AddLegalEntity(DbContext, new FormattingService());
            var testLegalEntity = TestsModel.LegalEntity;
            var testLegalEntityId = addLegalEntityTask.DoTask(testLegalEntity);
            Assert.IsTrue(testLegalEntityId.Data.HasValue);

            var task = new GetLegalEntity(DbContext);
            var result = task.DoTask(testLegalEntityId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var legalEntity = result.Data;
            Assert.IsNotNull(legalEntity);

            Assert.AreEqual(testLegalEntity.Name, legalEntity.Name);
            Assert.AreEqual(testLegalEntity.TradeName, legalEntity.TradeName);
            Assert.AreEqual(testLegalEntity.TaxId, legalEntity.TaxId);
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
            var task = new GetLegalEntity(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
