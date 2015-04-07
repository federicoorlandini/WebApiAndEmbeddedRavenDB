using Autofac;
using Autofac.Integration.WebApi;
using EmbeddedRavenDB.DataAccess;
using EmbeddedRavenDB.Models;
using Microsoft.Owin;
using Owin;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace EmbeddedRavenDB.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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
            // Controller registrations
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).
                Where(type => !type.IsAbstract && typeof(ApiController).IsAssignableFrom(type));

            // Document store registration
            builder.RegisterType<DocumentStoreFactory>().
                As<IDocumentStoreFactory>();

            // Repository registration
            const string ravenDBUrl = "http://localhost:8080";
            const string databaseName = "EmbeddedRavenDBTest";

            builder.RegisterType<Repository<Customer>>().
            As<IRepository<Customer>>().
            WithParameters(new[] {
                    new NamedParameter("url", ravenDBUrl),
                    new NamedParameter("databaseName", databaseName)
                });

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;

            // Migrate data
            var ravenMigrationOptions = new RavenMigrations.MigrationOptions
            {
                Direction = RavenMigrations.Directions.Up
            };

            using (var documentStore = new DocumentStore() { Url = ravenDBUrl, DefaultDatabase = databaseName })
            {
                documentStore.Initialize();
                RavenMigrations.Runner.Run(documentStore, ravenMigrationOptions);
            }

            // Done
            app.UseWebApi(config);
        }
    }
}