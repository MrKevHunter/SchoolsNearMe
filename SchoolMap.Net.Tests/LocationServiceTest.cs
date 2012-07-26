using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolMap.Net.Services;

namespace SchoolMap.Net.Tests
{
    [TestClass]
    public class LocationServiceTest
    {
        [TestMethod]
        public void GetLocationLocalhostParsingTest()
        {
            var service = new HostIpLocationService();
            var ipaddress = System.Net.IPAddress.Parse("127.0.0.1");
            var result = service.GetLocationByIpAddress(ipaddress);
            Assert.AreEqual((decimal)-88.4588d, result.Longitude);
            Assert.AreEqual((decimal)41.7696, result.Latitude);
        }
    }
}
