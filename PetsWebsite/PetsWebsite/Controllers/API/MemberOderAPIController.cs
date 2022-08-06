using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/MemberOder/{action}")]
    [ApiController]
    public class MemberOderAPIController : ControllerBase
    {
        private readonly PetsDBContext _petsDB;

        public MemberOderAPIController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }
        [HttpGet]
        public MemberOrderViewModel GetMemberOrderInfo()
        {
            if (HttpContext.Session.GetString("TraderOrderNo") == null)
            {
                return null;
            }
            var OrderInfo = _petsDB.Orders.Where(o => o.OrderId == HttpContext.Session.GetString("TraderOrderNo"))
                .Select(o=>new MemberOrderViewModel()
                {
                    OrderId= o.OrderId,
                    TradeNo= o.MerchantId,
                    Amount= o.Amount,
                    PaymentDate= DateTime.Parse(o.PayDate.ToString()).ToString("G"),
                    PaymentWay= o.PaymentWay,
                    OrderDate= DateTime.Parse(o.OrderDate.ToString()).ToString("G")
                }).FirstOrDefault();
            return OrderInfo;
        }
        [HttpGet]
        public List<MemberBuyProductViewModel> GetMemberProductInfo()
        {
            if (HttpContext.Session.GetString("TraderOrderNo") == null)
            {
                return null;
            }
            var PurchaseProductInfo = _petsDB.OrderDetails.Where(o => o.OrderId == HttpContext.Session.GetString("TraderOrderNo"))
                .Select(o => new MemberBuyProductViewModel()
                {
                    ProductName=o.Product.ProductName,
                    PurchaseCount=o.Counts

                }).ToList();
            return PurchaseProductInfo;
        }
        [HttpGet]
        public List<OrderHistory> GetMemberOrderHistory()
        {
            var query = _petsDB.Orders.Where(o => o.UserId == 4).Select(o => new OrderHistory()
            {
                OrderId = o.OrderId,
                OrderDate = DateTime.Parse(o.OrderDate.ToString()).ToString(),
                OrderWay = o.PaymentWay,
                OrderStatus = o.OrderStatusNumber,
                OrderDetails = o.OrderDetails.ToList().Select(x => new OrderProduct
                {
                    ProductName = x.Product.ProductName,
                    Price = x.Product.UnitPrice,
                    Count = x.Counts
                })
            }).ToList();
            return query;
        }
    }
    public class OrderHistory
    {

        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public string OrderWay { get; set; }
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
