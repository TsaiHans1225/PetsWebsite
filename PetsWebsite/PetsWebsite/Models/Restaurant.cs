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
        public string? PhotoPath { get; set; }
        public string? Phone { get; set; }
        public int? CompanyId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public bool? State { get; set; }

        public virtual Company? Company { get; set; }
    }
}
