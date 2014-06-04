using System;
using ITourist.Models.LogicModels.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITourist.Tests.Models.Services
{
    [TestClass]
    public class SecurityManagerTest
    {
        [TestMethod]
        public void Password()
        {
            Assert.IsNotNull(SecurityManager.GetHashString("210194dima"));
        }
    }
}
