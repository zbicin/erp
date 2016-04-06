using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Models
{
    public class OrderElement
    {
        public int Id { get; set; }
        public String ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
