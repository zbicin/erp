using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Location { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ApplicationUser> Workers { get; set; } 

    }
}