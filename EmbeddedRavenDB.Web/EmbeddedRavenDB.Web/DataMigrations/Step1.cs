using EmbeddedRavenDB.Models;
using RavenMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmbeddedRavenDB.Web.DataMigrations
{
    [Migration(1)]
    public class Step1 : Migration
    {
        public override void Up()
        {
            // Adding Customers
            var customers = new[] {
                new Customer { Name = "Customer1" },
                new Customer { Name = "Customer2" },
                new Customer { Name = "Customer3" },
                new Customer { Name = "Customer4" }
            };

            using(var session = DocumentStore.OpenSession())
            {
                foreach (var customer in customers)
                {
                    session.Store(customer);
                }
                session.SaveChanges();
            }
        }
    }
}