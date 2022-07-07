using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
    }
}
