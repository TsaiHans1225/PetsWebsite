using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class UserLogin
    {
        public int UserId { get; set; }
        public string LoginProvider { get; set; } = null!;
        public string ProviderKey { get; set; } = null!;
        public string? Account { get; set; }
        public string? Password { get; set; }
        public bool? Verification { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
