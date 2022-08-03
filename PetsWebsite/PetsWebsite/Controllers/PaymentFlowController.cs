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
        public PaymentFlowController(PetsDBContext petsDB,Setting setting)
        {
            _petsDB = petsDB;
            _setting = setting;
        }
        [HttpPost]
        public IActionResult GetPayFlowData(OrderInfo orderInfo)
        {
            TradeInfo tradeInfo = new TradeInfo()
            {
                MerchantID = _setting.MerchantID,
                RespondType = "String",
                TimeStamp = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString(),
                Version = _setting.Version,
                MerchantOrderNo = $"T{User.GetId()}_{DateTime.Now.ToString("yyyyMMddHHmm")}",
                Amt = orderInfo.OrderSum,
                ItemDesc = orderInfo.OrderDesc,
                ExpireDate = null,
                ReturnURL = _setting.ReturnURL,
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
            var TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara,_setting.HashKey , _setting.HashIV);
            // SHA256 加密
            var TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_setting.HashKey}&{TradeInfo}&HashIV={_setting.HashIV}");
            NewebPayModel NewebPayData = new NewebPayModel() {
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
            string HashKey = _setting.HashKey;
            string HashIV = _setting.HashIV;
            string TradeInfoDecrypt = CryptoUtil.DecryptAESHex(Request.Form["TradeInfo"], HashKey, HashIV);
            NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(TradeInfoDecrypt);
            //SpgatewayOutputDataModel convertModel = LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));
            return Redirect("/home/index");
        }
    }
}
