using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Articles = new HashSet<Article>();
        }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Introduction { get; set; }
        public string? PhotoPath { get; set; }
        public string? Phone { get; set; }
        public string? BusyTime { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? Reserve { get; set; }
        public int? CompanyId { get; set; }
        public bool? State { get; set; }
        public string? AuditResult { get; set; }

        public virtual Company? Company { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
