using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.LegalEntityTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class ListLegalEntityTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addLegalEntity = new AddLegalEntity(DbContext, new FormattingService());
            var testLegalEntity1 = TestsModel.LegalEntity;
            var testLegalEntity1Id = addLegalEntity.DoTask(testLegalEntity1);
            Assert.IsTrue(testLegalEntity1Id.Data.HasValue);
            addLegalEntity = new AddLegalEntity(DbContext, new FormattingService());
            var testLegalEntity2 = TestsModel.LegalEntity;
            var testLegalEntity2Id = addLegalEntity.DoTask(testLegalEntity2);
            Assert.IsTrue(testLegalEntity2Id.Data.HasValue);

            var task = new ListLegalEntities(DbContext);
            var result = task.DoTask(null);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var legalEntities = result.Data;
            Assert.IsNotNull(legalEntities);
            Assert.IsTrue(legalEntities.Count >= 2);

            var legalEntity1 = legalEntities.SingleOrDefault(le => le.Id == testLegalEntity1Id.Data.Value);
            Assert.IsNotNull(legalEntity1);
            Assert.AreEqual(testLegalEntity1.Name, legalEntity1.Name);
            Assert.AreEqual(testLegalEntity1.TradeName, legalEntity1.TradeName);
            Assert.AreEqual(testLegalEntity1.TaxId, legalEntity1.TaxId);
            Assert.AreEqual(testLegalEntity1.Email, legalEntity1.Email);
            Assert.IsNotNull(legalEntity1.Address);
            Assert.AreEqual(testLegalEntity1.Address.Street, legalEntity1.Address.Street);
            Assert.AreEqual(testLegalEntity1.Address.City, legalEntity1.Address.City);
            Assert.AreEqual(testLegalEntity1.Address.Region, legalEntity1.Address.Region);
            Assert.AreEqual(testLegalEntity1.Address.PostalCode, legalEntity1.Address.PostalCode);
            Assert.IsNotNull(legalEntity1.Address.Country);
            Assert.AreEqual(testLegalEntity1.Address.Country.Name, legalEntity1.Address.Country.Name);
            Assert.AreEqual(testLegalEntity1.Address.Country.IsoCode, legalEntity1.Address.Country.IsoCode);

            var legalEntity2 = legalEntities.SingleOrDefault(le => le.Id == testLegalEntity2Id.Data.Value);
            Assert.IsNotNull(legalEntity2);
            Assert.AreEqual(testLegalEntity2.Name, legalEntity2.Name);
            Assert.AreEqual(testLegalEntity2.TradeName, legalEntity2.TradeName);
            Assert.AreEqual(testLegalEntity2.TaxId, legalEntity2.TaxId);
            Assert.AreEqual(testLegalEntity2.Email, legalEntity2.Email);
            Assert.IsNotNull(legalEntity2.Address);
            Assert.AreEqual(testLegalEntity2.Address.Street, legalEntity2.Address.Street);
            Assert.AreEqual(testLegalEntity2.Address.City, legalEntity2.Address.City);
            Assert.AreEqual(testLegalEntity2.Address.Region, legalEntity2.Address.Region);
            Assert.AreEqual(testLegalEntity2.Address.PostalCode, legalEntity2.Address.PostalCode);
            Assert.IsNotNull(legalEntity2.Address.Country);
            Assert.AreEqual(testLegalEntity2.Address.Country.Name, legalEntity2.Address.Country.Name);
            Assert.AreEqual(testLegalEntity2.Address.Country.IsoCode, legalEntity2.Address.Country.IsoCode);

            var removeLegalEntity = new RemoveLegalEntity(DbContext);
            var removeResult1 = removeLegalEntity.DoTask(legalEntity1);
            var removeResult2 = removeLegalEntity.DoTask(legalEntity2);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListLegalEntities(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
