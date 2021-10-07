using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Storage.Tests
{
    [TestClass]
    public class StorageContainerTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            var publisher = new Publisher { Id = 100 };
            var container = new StorageContainer(publisher);
            Assert.AreEqual("publisher-100", container.Name);

            var recordLabel = new RecordLabel { Id = 110 };
            container = new StorageContainer(recordLabel);
            Assert.AreEqual("record-label-110", container.Name);
        }
    }
}
