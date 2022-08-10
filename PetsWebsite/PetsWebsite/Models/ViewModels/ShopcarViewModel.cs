namespace PetsWebsite.Models.ViewModels
{
    public class ShopcarViewModel
    {
        public string ProductName { get; set; }
        public string PicturePath { get; set; }
        public int? Count { get; set; }
        public decimal? Price { get; set; }
        public int ShopCarId { get; set; }
        public short? UnitsInStock { get; set; }
    }
}
