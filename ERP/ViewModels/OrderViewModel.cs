using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Models;

namespace ERP.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; } // czy skompletowane i gotowe do wysyłki, a nie "zakończone"
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CanceledAt { get; set; }

        public List<OrderElement> SelectedItems { get; set; }
        public IEnumerable<SelectListItem> AvailableItems { get; set; }
    }
}