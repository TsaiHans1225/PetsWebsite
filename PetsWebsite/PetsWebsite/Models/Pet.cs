using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Pet
    {
        public int PetId { get; set; }
        public int UserId { get; set; }
        public string PetName { get; set; } = null!;
        public string Species { get; set; } = null!;
        public int? Age { get; set; }
        public bool? Gender { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
