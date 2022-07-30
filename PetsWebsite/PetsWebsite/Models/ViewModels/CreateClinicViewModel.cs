namespace PetsWebsite.Models.ViewModels
{
    public class CreateClinicViewModel
    {
        public string ClinicName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string? Describe { get; set; }
        public string Emergency { get; set; }
    }
}
