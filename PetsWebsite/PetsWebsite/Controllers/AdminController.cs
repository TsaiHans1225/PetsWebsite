using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers
{
    [Route("{controller}/{action}")]
    public class AdminController : Controller
    {
        private readonly PetsDBContext _petsDB;

        public AdminController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }

        // 審核頁面
        public IActionResult AuditPage()
        {
            return View();
        }

        // 取得待審核商品
        [HttpGet]
        public List<Product> GetAuditProduct()
        {
            var query = _petsDB.Products.Where(p => p.State == false).Select(p => p).ToList();
            return query;
        }

        // 審核商品頁面
        public IActionResult AuditProductPage()
        {
            return View();
        }

        // 取得單一審核商品資料
        [HttpGet]
        [Route("{ProductId}")]
        public bool StartAuditProduct(int ProductId)
        {
            try
            {
                var auditProduct = _petsDB.Products.FirstOrDefault(p => p.ProductId == ProductId);
                HttpContext.Session.SetString("AuditProduct", JsonConvert.SerializeObject(auditProduct));
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // 取回審核商品資料
        [HttpGet]
        public Product GetCurrentAuditProduct()
        {
            return JsonConvert.DeserializeObject<Product>(HttpContext.Session.GetString("AuditProduct"));
        }

        // 存取修改建議
        [HttpPost]
        public bool SaveAuditSuggest(Product product)
        {
            var query = _petsDB.Products.First(p => p.ProductId == product.ProductId);
            query.AuditResult = product.AuditResult;
            try
            {
                _petsDB.SaveChanges();
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        // 允許上架
        [HttpGet]
        [Route("{ProductId}")]
        public IActionResult AllowPublish(int ProductId)
        {
            var query = _petsDB.Products.First(p => p.ProductId == ProductId);
            query.State = true;
            query.AuditResult = null;
            _petsDB.SaveChanges();
            return RedirectToAction("AuditPage");
        }
    }
}
