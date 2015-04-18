using Autofac;
using Autofac.Integration.WebApi;
using EmbeddedRavenDB.DataAccess;
using EmbeddedRavenDB.Models;
using EmbeddedRavenDB.Web.DataMigrations;
using Microsoft.Owin;
using Owin;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(EmbeddedRavenDB.Web.App_Start.Startup))]

namespace EmbeddedRavenDB.Web.App_Start
{
    public  class Startup
    {
        private const string ravenDBUrl = "http://localhost:8080";
        private const string databaseName = "EmbeddedRavenDBTest";

        protected readonly IDataMigrator _dataMigrator;

        public Startup()
        {
            _dataMigrator = new DataMigrator();
        }

        public void Configuration(IAppBuilder app)
        {
            var documentStore = InstantiateRavenDBDocumentStore();

            // Web API configuration and services
            var config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Autofac configuration
            AutoFacConfiguration(config, documentStore);

            // Migrate data
            _dataMigrator.Migrate(documentStore);

            // Done
            app.UseWebApi(config);
        }

        /// <summary>
        /// Create an instance of DocumentStore that point to the real RavenDB server
        /// </summary>
        /// <returns></returns>
        protected virtual IDocumentStore InstantiateRavenDBDocumentStore()
        {
            var documentStore = new DocumentStore()
            {
                Url = ravenDBUrl,
                DefaultDatabase = databaseName
            };
            documentStore.Initialize();
            return documentStore;
        }

        // Autofac configuration
        protected void AutoFacConfiguration(HttpConfiguration config, IDocumentStore documentStore)
        {
            // Controller registrations
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).
                Where(type => !type.IsAbstract && typeof(ApiController).IsAssignableFrom(type));

            // Let's register the DocumentStore that point to the real RavenDB server
            builder.RegisterInstance(documentStore).As<IDocumentStore>();

            // Repository registration
            builder.RegisterType<Repository<Customer>>().As<IRepository<Customer>>();

            // Replacement
            AutofacReplacement(builder, documentStore);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }

        /// <summary>
        /// Override this if need to replace some registrations in the Autofac container
        /// </summary>
        /// <param name="config"></param>
        protected virtual void AutofacReplacement(ContainerBuilder builder, IDocumentStore documentStore)
        {
            // Nohting to replace here
        }
    }
}