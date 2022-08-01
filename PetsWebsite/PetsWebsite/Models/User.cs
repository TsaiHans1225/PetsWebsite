using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class User
    {
        public User()
        {
            Collections = new HashSet<Collection>();
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
            Pets = new HashSet<Pet>();
            ShoppingCars = new HashSet<ShoppingCar>();
            UserLogins = new HashSet<UserLogin>();
        }

        public int UserId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Age { get; set; }
        public string UserName { get; set; } = null!;
        public int RoleId { get; set; }
        public string Email { get; set; } = null!;
        public string? Zipcode { get; set; }
        public string? Password { get; set; }
        public string? Account { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<ShoppingCar> ShoppingCars { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
