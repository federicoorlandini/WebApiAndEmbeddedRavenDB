using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedRavenDB.Models
{
    public class Customer : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
