using ITourist.Models.DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITourist.Tests.Models.DataModels
{
    [TestClass]
    public class CountryTest
    {
        [TestMethod]
        public void Contains()
        {
            var country = new Country { Id = 1, Translation = new Translation("Saudi Arabia", "Саудовская Аравия") };
            Assert.IsTrue(country.Contains("Sa", Culture.En));
            Assert.IsTrue(country.Contains("sa", Culture.En));
            Assert.IsTrue(country.Contains("Sa ar", Culture.En));
            Assert.IsTrue(country.Contains("са ар"));
            Assert.IsFalse(country.Contains("ddd"));
        }
    }
}
