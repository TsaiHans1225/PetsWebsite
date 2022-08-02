namespace PetsWebsite.Models.ViewModels
{
    public class RestaurantViewModel
    {
        public double dist { get; set; }
        public string City { get;  set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public string Region { get;  set; }
        public double? Longitude { get;  set; }
        public double? Latitude { get;  set; }
        public string Restaurants { get;  set; }
        public string? PhotoPath { get;  set; }
        public int RestaurantsId { get; internal set; }
    }
}