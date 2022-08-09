namespace PetsWebsite.Models.ViewModels
{
    public class OrderInfo
    {
        public List<OrderViewModle> OrderList { get; set; }
        public int OrderSum { get; set; }
        public string? Payment { get; set; }
        public string OrderDesc { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class OrderViewModle
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
