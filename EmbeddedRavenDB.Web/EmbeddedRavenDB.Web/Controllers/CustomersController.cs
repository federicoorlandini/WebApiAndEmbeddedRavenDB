using EmbeddedRavenDB.DataAccess;
using EmbeddedRavenDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmbeddedRavenDB.Web.Controllers
{
    [RoutePrefix("customers")]
    public class CustomersController : ApiController
    {
        private IRepository<Customer> _repository;

        public CustomersController(IRepository<Customer> customerRepository)
        {
            _repository = customerRepository;
        }

        [Route("all")]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
