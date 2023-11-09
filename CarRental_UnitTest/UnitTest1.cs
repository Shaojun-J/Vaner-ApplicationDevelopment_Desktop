using CarRental.Models;
using System.Net.Http.Json;
using CarRental.Models;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace CarRental_UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

       
        [Test]
        public async Task Test1()
        {
            Staff staff = new Staff();
            staff.user_name = "admin";
            staff.password = "admin123";
            var serverRes = await DBA.client.PostAsJsonAsync("StaffLogin", staff);
            var content = serverRes.Content.ReadAsStringAsync().Result;
            Response contentJson = JsonConvert.DeserializeObject<Response>(content.ToString());

            Assert.IsNotNull(contentJson);
            Assert.That(contentJson.statusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Test2()
        {
            Staff staff = new Staff();
            staff.user_name = "admin";
            staff.password = "admin1234";
            var serverRes = await DBA.client.PostAsJsonAsync("StaffLogin", staff);
            var content = serverRes.Content.ReadAsStringAsync().Result;
            Response contentJson = JsonConvert.DeserializeObject<Response>(content.ToString());

            Assert.IsNotNull(contentJson);
            Assert.That(contentJson.statusCode, Is.EqualTo(100));
        }
    }
}