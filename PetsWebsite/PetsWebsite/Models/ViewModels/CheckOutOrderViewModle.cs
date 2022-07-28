namespace PetsWebsite.Models.ViewModels
{
    public class CheckOutOrderViewModle
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Count { get; set; }
        public decimal? Price { get; set; }
        public string PhotoPath { get; set; }
    }
}
