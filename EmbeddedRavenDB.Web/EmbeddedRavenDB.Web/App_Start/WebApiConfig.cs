using Autofac;
using Autofac.Integration.WebApi;
using EmbeddedRavenDB.DataAccess;
using EmbeddedRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace EmbeddedRavenDB.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Autofac configuration
            // Controller registrations
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).
                Where(type => !type.IsAbstract && typeof(ApiController).IsAssignableFrom(type));

            // Document store registration
            builder.RegisterType<DocumentStoreFactory>().
                As<IDocumentStoreFactory>().
                WithParameters(new [] { 
                    new NamedParameter("Url", "http://localhost:8080"), 
                    new NamedParameter("Database", "EmbeddedRavenDBTest")
            });

            // Repository registration
            builder.RegisterType<Repository<Customer>>().
                As<IRepository<Customer>>();

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }
    }
}
