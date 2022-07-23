using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class ShoppingCar
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? Count { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
