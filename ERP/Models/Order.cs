﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? CompletedAt { get; set; } // czy skompletowane i gotowe do wysyłki, a nie "zakończone"
        [Column(TypeName = "datetime2")]
        public DateTime? ShippedAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DeliveredAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? CanceledAt { get; set; }

        public List<OrderElement> Elements { get; set; }

        public OrderStatus Status
        {
            get
            {
                if (CanceledAt.HasValue) return OrderStatus.Canceled;
                if (DeliveredAt.HasValue) return OrderStatus.Delivered;
                if (ShippedAt.HasValue) return OrderStatus.Shipped;
                if (CompletedAt.HasValue) return OrderStatus.Completed;
                return OrderStatus.Created;
            }
        }

        public enum OrderStatus
        {
            Created,
            Completed,
            Shipped,
            Delivered,
            Canceled
        }

    }
}