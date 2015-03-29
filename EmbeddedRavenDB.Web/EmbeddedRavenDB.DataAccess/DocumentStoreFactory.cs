using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedRavenDB.DataAccess
{
    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        public IDocumentStore Create(string url, string database)
        {
            var store = new DocumentStore() { 
                Url = url, 
                DefaultDatabase = database 
            };
            return store;
        }
    }
}
