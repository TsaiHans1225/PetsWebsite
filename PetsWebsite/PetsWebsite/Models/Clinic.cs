using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Clinic
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Region { get; set; } = null!;
        public bool? Emergency { get; set; }
        public string? Describe { get; set; }
    }
}
