using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.PersonTaskTests
{
    [TestClass]
    public class UpdatePersonTests : TestsBase
    {
        public void UpdatePersonModel(Person person)
        {
            var personUpdates = TestsModel.Person;
            person.FirstName = personUpdates.FirstName;
            person.MiddleName = personUpdates.MiddleName;
            person.LastName = personUpdates.LastName;
            person.NameSuffix = personUpdates.NameSuffix;
            person.Phone = TestsModel.PhoneNumber;
            person.Address = TestsModel.Address;
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testPerson = TestsModel.Person;
            var addPersonTask = new AddPerson(DbContext);
            var addPersonResult = addPersonTask.DoTask(testPerson);

            Assert.IsTrue(addPersonResult.Success);
            Assert.IsNull(addPersonResult.Exception);

            var task = new UpdatePerson(DbContext);
            var toUpdate = testPerson;
            UpdatePersonModel(toUpdate);
            var result = task.DoTask(toUpdate);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getPersonTask = new GetPerson(DbContext);
            var person = getPersonTask.DoTask(toUpdate.Id)?.Data;

            Assert.IsNotNull(person);
            Assert.AreEqual(toUpdate.FirstName, person.FirstName);
            Assert.AreEqual(toUpdate.MiddleName, person.MiddleName);
            Assert.AreEqual(toUpdate.LastName, person.LastName);
            Assert.AreEqual(toUpdate.NameSuffix, person.NameSuffix);
            Assert.AreEqual(toUpdate.Email, person.Email);
            Assert.AreEqual(toUpdate.Phone, person.Phone);
            Assert.AreEqual(toUpdate.Address.Street, person.Address.Street);
            Assert.AreEqual(toUpdate.Address.City, person.Address.City);
            Assert.AreEqual(toUpdate.Address.Region, person.Address.Region);
            Assert.AreEqual(toUpdate.Address.PostalCode, person.Address.PostalCode);
            Assert.AreEqual(toUpdate.Address.Country.Name, person.Address.Country.Name);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(person);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdatePerson(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
