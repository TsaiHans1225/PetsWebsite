namespace PetsWebsite.Models.ViewModels
{
    public class OrderInfo
    {
        public List<OrderViewModle> OrderList { get; set; }
        public int OrderSum { get; set; }
        public string? Payment { get; set; }
        public string OrderDesc { get; set; }
    }
    public class OrderViewModle
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
    }
}
