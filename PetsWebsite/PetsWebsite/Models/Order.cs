using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string OrderId { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? OrderStatusNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Amount { get; set; }
        public string? MerchantId { get; set; }
        public string? PaymentWay { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
