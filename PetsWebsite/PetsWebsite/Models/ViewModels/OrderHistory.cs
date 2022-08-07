namespace PetsWebsite.Models.ViewModels
{
    public class OrderHistory
    {
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public string OrderWay { get; set; }
        public decimal? Amount { get; set; }
        public int? OrderStatus { get; set; }
        public IEnumerable<OrderProduct> OrderDetails { get; set; }
    }
    public class OrderProduct
    {
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? Count { get; set; }
    }
}
