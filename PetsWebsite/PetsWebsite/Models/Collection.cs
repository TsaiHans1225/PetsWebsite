using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Collection
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime? CollectDate { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
