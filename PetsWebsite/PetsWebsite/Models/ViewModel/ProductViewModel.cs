namespace PetsWebsite.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public string ProductDescription { get; set; }
    }
}
