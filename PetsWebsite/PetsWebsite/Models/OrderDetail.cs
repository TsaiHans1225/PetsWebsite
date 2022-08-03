﻿using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class OrderDetail
    {
        public string OrderId { get; set; } = null!;
        public int ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? Counts { get; set; }
        public double? Discount { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
