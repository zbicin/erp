using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.ViewModels
{
    public class ReportViewModel
    {
        public List<Order> Orders { get; set; }
        public int Id { get; set; }
        
        public int CreatedOrders { get; set; }        
        public int CompletedOrders { get; set; }        
        public int ShippedOrders { get; set; }        
        public int DeliveredOrders { get; set; }        
        public int CanceledOrders { get; set; }

        public List<Item> Items { get; set; }
    }
}