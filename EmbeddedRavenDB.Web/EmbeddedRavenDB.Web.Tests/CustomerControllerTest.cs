using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin.Testing;
using FluentAssertions;
using System.Collections.Generic;
using EmbeddedRavenDB.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System.Linq;

namespace EmbeddedRavenDB.Web.Tests
{
    [TestClass]
    public class CustomerControllerTest
    {
        [TestMethod]
        public void GetAll_shouldReturnAllTheCustomers()
        {
            using(var server = TestServer.Create(app => {
                var startUp = new Startup();
                startUp.Configuration(app);
            }))
            {
                var client = server.HttpClient;
                var response = client.GetAsync("/customers/all").Result;

                var content = response.Content.ReadAsStringAsync().Result;

                var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(content);

                customers.Count().Should().Be(4);
            }
        }
    }
}
