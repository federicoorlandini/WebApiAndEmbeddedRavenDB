using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedRavenDB.Models
{
    public class Order : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
    }
}
