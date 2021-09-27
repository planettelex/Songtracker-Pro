using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.MerchandiseTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class ListMerchandiseCategoriesTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new ListMerchandiseCategories(DbContext);
            var allCategoriesResult = task.DoTask(null);

            Assert.IsTrue(allCategoriesResult.Success);
            Assert.IsNull(allCategoriesResult.Exception);
            Assert.IsNotNull(allCategoriesResult.Data);
            Assert.IsTrue(allCategoriesResult.Data.Any());
        }
        
        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListMerchandiseCategories(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
