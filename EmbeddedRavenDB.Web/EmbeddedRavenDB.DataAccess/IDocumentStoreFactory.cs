using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedRavenDB.DataAccess
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore Create(string url, string database);
    }
}
