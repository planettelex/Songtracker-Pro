using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.PersonTaskTests
{
    [TestClass]
    public class AddPersonTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddPerson(DbContext);
            var testPerson = TestsModel.Person;
            var result = task.DoTask(testPerson);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var personId = result.Data;
            Assert.IsNotNull(personId);
            Assert.IsTrue(personId > 0);

            var getPersonTask = new GetPerson(DbContext);
            var person = getPersonTask.DoTask(personId.Value)?.Data;

            Assert.IsNotNull(person);
            Assert.AreEqual(testPerson.FirstName, person.FirstName);
            Assert.AreEqual(testPerson.MiddleName, person.MiddleName);
            Assert.AreEqual(testPerson.LastName, person.LastName);
            Assert.AreEqual(testPerson.NameSuffix, person.NameSuffix);
            Assert.AreEqual(testPerson.Email, person.Email);
            Assert.AreEqual(testPerson.Phone, person.Phone);
            Assert.IsNotNull(testPerson.Address);
            Assert.AreEqual(testPerson.Address.Street, person.Address.Street);
            Assert.AreEqual(testPerson.Address.City, person.Address.City);
            Assert.AreEqual(testPerson.Address.Region, person.Address.Region);
            Assert.AreEqual(testPerson.Address.PostalCode, person.Address.PostalCode);
            Assert.IsNotNull(testPerson.Address.Country);
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
            var task = new AddPerson(EmptyDbContext);
            var result = task.DoTask(new Person());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
