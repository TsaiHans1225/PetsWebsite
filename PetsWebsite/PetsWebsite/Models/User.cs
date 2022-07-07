using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Pets = new HashSet<Pet>();
        }

        public int UserId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Region { get; set; }
        public string Address { get; set; } = null!;
        public bool Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int? Age { get; set; }
        public string UserName { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
