using Raven.Client;
using System;
namespace EmbeddedRavenDB.Web.DataMigrations
{
    public interface IDataMigrator
    {
        void Migrate(IDocumentStore documentStore);
    }
}
