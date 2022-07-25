using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Company
    {
        public Company()
        {
            Clinics = new HashSet<Clinic>();
            Products = new HashSet<Product>();
            Restaurants = new HashSet<Restaurant>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ContactPerson { get; set; }

        public virtual CompanyAccount CompanyAccount { get; set; } = null!;
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
