namespace PetsWebsite.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public string? PhotoPath { get; set; }
        public string? Describe { get; set; }
        public DateTime? LaunchDate { get; set; }
        public DateTime? RemoveDate { get; set; }
        public int? CompanyId { get; set; }
        public bool? State { get; set; }
        public string? Discount { get; set; }
        public string? Species { get; set; }
        public string? AuditResult { get; set; }
    }
}
