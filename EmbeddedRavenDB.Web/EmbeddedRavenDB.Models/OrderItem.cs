using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedRavenDB.Models
{
    public class OrderItem : IEntity
    {
        public int ID { get; set; }
        public string Article { get; set; }
        public int Quantity { get; set; }
    }
}
