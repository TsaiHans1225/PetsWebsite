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

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int OrderStatusNumber { get; set; }
        public string Address { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
