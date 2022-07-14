namespace PetsWebsite.Models.ViewModel
{
    public class UserDetailsViewModel
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Region { get; set; }
        public string Address { get; set; } = null!;
        public string Birthday { get; set; }
    }
}
