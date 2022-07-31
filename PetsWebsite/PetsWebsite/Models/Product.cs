using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Product
    {
        public Product()
        {
            Collections = new HashSet<Collection>();
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCars = new HashSet<ShoppingCar>();
        }

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
        public bool? Species { get; set; }
        public string? AuditResult { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Company? Company { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCar> ShoppingCars { get; set; }
    }

    public class ShopPagedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int PageTotal { get; set; }

        public bool HasPrevPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < PageTotal;


        public ShopPagedList(IList<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageTotal = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static ShopPagedList<T> Create(IList<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new ShopPagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
