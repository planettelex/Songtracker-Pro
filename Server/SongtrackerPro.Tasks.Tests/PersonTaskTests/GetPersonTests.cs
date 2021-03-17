using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.PersonTaskTests
{
    [TestClass]
    public class GetPersonTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPersonTask = new AddPerson(DbContext);
            var testPerson = TestModel.Person;
            var testPersonId = addPersonTask.DoTask(testPerson);
            Assert.IsTrue(testPersonId.Data.HasValue);

            var task = new GetPerson(DbContext);
            var result = task.DoTask(testPersonId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var person = result.Data;
            Assert.IsNotNull(person);
            Assert.AreEqual(testPerson.FirstName, person.FirstName);
            Assert.AreEqual(testPerson.MiddleName, person.MiddleName);
            Assert.AreEqual(testPerson.LastName, person.LastName);
            Assert.AreEqual(testPerson.NameSuffix, person.NameSuffix);
            Assert.AreEqual(testPerson.Email, person.Email);
            Assert.AreEqual(testPerson.Phone, person.Phone);
            Assert.IsNotNull(person.Address);
            Assert.AreEqual(testPerson.Address.Street, person.Address.Street);
            Assert.AreEqual(testPerson.Address.City, person.Address.City);
            Assert.AreEqual(testPerson.Address.Region, person.Address.Region);
            Assert.AreEqual(testPerson.Address.PostalCode, person.Address.PostalCode);
            Assert.IsNotNull(person.Address.Country);
            Assert.AreEqual(testPerson.Address.Country.Name, person.Address.Country.Name);
            Assert.AreEqual(testPerson.Address.Country.IsoCode, person.Address.Country.IsoCode);

            var removePersonTask = new RemovePerson(DbContext);
            var removeResult = removePersonTask.DoTask(person);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new GetRecordLabel(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
