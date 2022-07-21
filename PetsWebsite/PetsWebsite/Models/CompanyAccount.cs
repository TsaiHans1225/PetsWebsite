using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class CompanyAccount
    {
        public int CompanyId { get; set; }
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
    }
}
