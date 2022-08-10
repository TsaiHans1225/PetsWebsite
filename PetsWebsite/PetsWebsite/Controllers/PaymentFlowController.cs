using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;
using PetsWebsite.Utility;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace PetsWebsite.Controllers
{
    public class PaymentFlowController : Controller
    {
        private readonly PetsDBContext _petsDB;
        private readonly Setting _setting;
        public PaymentFlowController(PetsDBContext petsDB, Setting setting)
        {
            _petsDB = petsDB;
            _setting = setting;
        }
        [HttpPost]
        public async Task<IActionResult> GetPayFlowData(OrderInfo orderInfo)
        {
            var DeleteCollect = _petsDB.ShoppingCars.Where(p => orderInfo.OrderList.Select(o => o.ProductId).ToList().Contains(p.ProductId) && p.UserId == User.GetId()).ToList();
            _petsDB.RemoveRange(DeleteCollect);
            string OrderNo = $"T{User.GetId()}_{DateTime.Now.ToString("yyyyMMddHHmm")}";
            _petsDB.Orders.Add(new Order()
            {
                OrderId = OrderNo,
                UserId = User.GetId(),
                OrderDate = DateTime.Now,
                Email = User.GetMail(),
                OrderStatusNumber = 0,
                Amount = orderInfo.OrderSum,
                PaymentWay = orderInfo.Payment,
                Address = orderInfo.Address,
                Phone = orderInfo.Phone,
            });
            foreach (var item in orderInfo.OrderList)
            {
                await _petsDB.OrderDetails.AddAsync(new OrderDetail()
                {
                    OrderId = OrderNo,
                    ProductId = item.ProductId,
                    UnitPrice = item.Price,
                    Counts = item.Count,
                });
            }
            try
            {
                _petsDB.SaveChanges();
            }
            catch (Exception)
            {

            }
            TradeInfo tradeInfo = new TradeInfo()
            {
                MerchantID = _setting.MerchantID,
                RespondType = "String",
                TimeStamp = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString(),
                Version = _setting.Version,
                MerchantOrderNo = OrderNo,
                Amt = orderInfo.OrderSum,
                ItemDesc = orderInfo.OrderDesc,
                ExpireDate = null,
                ReturnURL = new StringBuilder().Append(HttpContext.Request.Scheme).Append("://").Append(HttpContext.Request.Host).ToString() + _setting.ReturnURL,
                NotifyURL = _setting.NotifyURL,
                CustomerURL = _setting.CustomerURL,
                ClientBackURL = null,
                Email = User.GetMail(),
                EmailModify = 0,
            };
            switch (orderInfo.Payment)
            {
                case "CREDIT":
                    tradeInfo.CREDIT = 1;
                    break;
                case "WEBATM":
                    tradeInfo.WEBATM = 1;
                    break;
                default:
                    break;
            }
            var tradeQueryPara = string.Join("&", tradeInfo.ToKvpList<TradeInfo>().Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            var TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _setting.HashKey, _setting.HashIV);
            // SHA256 加密
            var TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_setting.HashKey}&{TradeInfo}&HashIV={_setting.HashIV}");
            NewebPayModel NewebPayData = new NewebPayModel()
            {
                MerchantID = _setting.MerchantID,
                TradeInfo = TradeInfo,
                TradeSha = TradeSha,
                Version = _setting.Version
            };
            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            return Json(NewebPayData);
        }
        [HttpPost]
        public IActionResult CallbackReturn()
        {
            string TradeInfoDecrypt = CryptoUtil.DecryptAESHex(Request.Form["TradeInfo"], _setting.HashKey, _setting.HashIV);
            NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(TradeInfoDecrypt);
            var Status = decryptTradeCollection["Status"];
            var Amt = decryptTradeCollection["Amt"];
            var OrderID = decryptTradeCollection["MerchantOrderNo"];
            var TradeNo = decryptTradeCollection["TradeNo"];
            var PayTime = decryptTradeCollection["PayTime"];
            HttpContext.Session.SetString("TraderOrderNo", OrderID);
            var MemberOrder = _petsDB.Orders.Find(OrderID);
            MemberOrder.MerchantId = TradeNo;
            if (Status != "SUCCESS")
            {
                _petsDB.SaveChanges();
                return Redirect("/PayResult/PayFalse");
            }
            MemberOrder.PayDate = DateTime.Parse(PayTime);
            MemberOrder.OrderStatusNumber = 1;
            _petsDB.SaveChanges();
            return Redirect("/PayResult/PayResult");
        }
    }
}
