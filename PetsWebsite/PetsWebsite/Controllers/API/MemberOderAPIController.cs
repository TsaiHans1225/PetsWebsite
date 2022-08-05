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
            DateTime sample_date = new DateTime(2010, 6, 14);
            sample_date.ToString();
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
    }
}
