using ITourist.Models.DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITourist.Tests.Models.DataModels
{
    [TestClass]
    public class RegionTest
    {
        [TestMethod]
        public void Contains()
        {
            var region = new Region { Id = 1, Translation = new Translation("Saudi Arabia", "Саудовская Аравия") };
            Assert.IsTrue(region.Contains("Sa", Culture.En));
            Assert.IsTrue(region.Contains("sa", Culture.En));
            Assert.IsTrue(region.Contains("Sa ar", Culture.En));
            Assert.IsTrue(region.Contains("са ар"));
            Assert.IsFalse(region.Contains("ddd"));
        }
    }
}
