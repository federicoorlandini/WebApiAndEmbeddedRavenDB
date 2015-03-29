using EmbeddedRavenDB.Models;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedRavenDB.DataAccess
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private IDocumentStore _documentStore;
        
        public Repository(IDocumentStoreFactory documentStoreFactory, string url, string databaseName)
        {
            _documentStore = documentStoreFactory.Create(url, databaseName);
            _documentStore.Initialize();
        }

        public IEnumerable<T> GetAll()
        {
            using(var session = _documentStore.OpenSession())
            {
                return session.Query<T>().AsEnumerable();
            }
        }

        public T GetById(int id)
        {
            using(var session = _documentStore.OpenSession())
            {
                return session.Load<T>(id.ToString());
            }
        }

        public T Add(T entity)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
                return entity;
            }
        }
    }
}
