using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmbeddedRavenDB.Web.DataMigrations
{
    public class DataMigrator : EmbeddedRavenDB.Web.DataMigrations.IDataMigrator
    {
        public void Migrate(IDocumentStore documentStore)
        {
            // Migrate data
            var ravenMigrationOptions = new RavenMigrations.MigrationOptions
            {
                Direction = RavenMigrations.Directions.Up
            };

            RavenMigrations.Runner.Run(documentStore, ravenMigrationOptions);
        }
    }
}