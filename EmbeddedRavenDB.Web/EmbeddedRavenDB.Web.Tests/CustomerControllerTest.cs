using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin.Testing;
using FluentAssertions;
using System.Collections.Generic;
using EmbeddedRavenDB.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System.Linq;
using EmbeddedRavenDB.Web.App_Start;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using EmbeddedRavenDB.DataAccess;
using Raven.Client.Embedded;
using Raven.Database.Server;
using Owin;
using Raven.Client;

namespace EmbeddedRavenDB.Web.Tests
{
    [TestClass]
    public class CustomerControllerTest
    {
        [TestMethod]
        public void GetAll_shouldReturnAllTheCustomers()
        {
            using(var server = TestServer.Create(app => {
                var startUp = new StartupForTesting();
                startUp.Configuration(app);
            }))
            {
                var client = server.HttpClient;

                var response = client.GetAsync("/customers/all").Result;

                var content = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine("Content: " + content);

                var customers = JsonConvert.DeserializeObject<Customer[]>(content);

                customers.Count().Should().Be(4);
            }
        }
    }

    public class StartupForTesting : Startup
    {
        public const string DatabaseName = "Test";
        public const string Url = "http://localhost:8081/";

        public StartupForTesting()
        {
            
        }

        protected override IDocumentStore InstantiateRavenDBDocumentStore()
        {
            var documentStore = new EmbeddableDocumentStore()
            {
                RunInMemory = true,
                UseEmbeddedHttpServer = true
            };
            documentStore.Configuration.Port = 8081;
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8081);

            documentStore.Initialize();

            return documentStore;
        }

        protected override void AutofacReplacement(ContainerBuilder builder, IDocumentStore documentStore)
        {
            // Replace the registration for the IDocumentStore beacuse we mus use the InMemory version
            builder.RegisterInstance(documentStore).As<IDocumentStore>();

            // Replace the registration for the IRepository<Customer>
            builder.RegisterType<Repository<Customer>>().As<IRepository<Customer>>();
        }
    }
}
