﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class Item
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int QuantityInStock { get; set; }
    }
}