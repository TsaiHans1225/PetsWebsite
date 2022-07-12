using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class UserAccount
    {
        public int UserId { get; set; }
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
