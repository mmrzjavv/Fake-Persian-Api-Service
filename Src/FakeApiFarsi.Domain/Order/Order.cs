using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeApiFarsi.Domain.Order
{
    public class Order
    {
        public int ID { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}
