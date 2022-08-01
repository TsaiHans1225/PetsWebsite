namespace PetsWebsite.Models.ViewModels
{
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
