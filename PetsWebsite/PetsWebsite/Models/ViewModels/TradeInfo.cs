namespace PetsWebsite.Models.ViewModels
{
    public class TradeInfo
    {
        public string MerchantID { get; set; }
        public string RespondType { get; set; }
        public string TimeStamp { get; set; }
        public string Version { get; set; }
        public string MerchantOrderNo { get; set; }
        public int Amt { get; set; }
        public string ItemDesc { get; set; }
        public string ExpireDate { get; set; }
        public string ReturnURL { get; set; }
        public string NotifyURL { get; set; }
        public string CustomerURL { get; set; }
        public string ClientBackURL { get; set; }
        public string Email { get; set; }
        public int EmailModify { get; set; }
        public int? CREDIT { get; set; }
        public int? WEBATM { get; set; }
    }
}
