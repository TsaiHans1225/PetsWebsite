namespace PetsWebsite.Models.ViewModels
{
    public class MemberOrderViewModel
    {
        public string OrderId { get; set; }
        public string TradeNo { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentWay { get; set; }
        public string OrderDate { get; set; }
    }
}
