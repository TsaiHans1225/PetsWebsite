﻿using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Product
    {
        public Product()
        {
            Collections = new HashSet<Collection>();
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCars = new HashSet<ShoppingCar>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public string? PhotoPath { get; set; }
        public string? Describe { get; set; }
        public DateTime? LaunchDate { get; set; }
        public DateTime? RemoveDate { get; set; }
        public int? CompanyId { get; set; }
        public bool? State { get; set; }
        public string? Discount { get; set; }
        public bool? Species { get; set; }
        public string? AuditResult { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Company? Company { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCar> ShoppingCars { get; set; }
    }
}
