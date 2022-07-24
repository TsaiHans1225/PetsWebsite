﻿using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Company
    {
        public Company()
        {
            Products = new HashSet<Product>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ContactPerson { get; set; }

        public virtual CompanyAccount CompanyAccount { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
