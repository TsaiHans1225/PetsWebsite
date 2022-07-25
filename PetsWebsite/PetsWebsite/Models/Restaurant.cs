using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Introduction { get; set; }
        public string? Photo { get; set; }
        public string Phone { get; set; } = null!;
        public int? CompanyId { get; set; }

        public virtual Company? Company { get; set; }
    }
}
