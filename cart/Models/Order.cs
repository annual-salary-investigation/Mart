using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mart.Models
{
    public class Order
    {
        public int ProductId { get; set; }
        public string Id { get; set; }
        public string Price { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
    }
}
