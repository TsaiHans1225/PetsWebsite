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

        // 取得待審核餐廳
        [HttpGet]
        public List<Restaurant> GetAuditRestaurant()
        {
            var query = _petsDB.Restaurants.Where(r => r.State == false).Select(r => r).ToList();
            return query;
        }

        // 審核商品頁面
        public IActionResult AuditProductPage()
        {
            return View();
        }

        // 審核餐廳頁面
        public IActionResult AuditRestaurantPage()
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
                var auditProduct = _petsDB.Products.First(p => p.ProductId == ProductId);
                HttpContext.Session.SetString("AuditProduct", JsonConvert.SerializeObject(auditProduct));
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // 取得單一審核商品資料
        [HttpGet]
        [Route("{RestaurantId}")]
        public bool StartAuditRestaurant(int RestaurantId)
        {
            try
            {
                var auditRestaurant = _petsDB.Restaurants.First(r => r.RestaurantId == RestaurantId);
                HttpContext.Session.SetString("AuditRestaurant", JsonConvert.SerializeObject(auditRestaurant));
                return true;
            }
            catch(Exception e)
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

        // 取回審核餐廳資料
        [HttpGet]
        public Restaurant GetCurrentAuditRestaurant()
        {
            return JsonConvert.DeserializeObject<Restaurant>(HttpContext.Session.GetString("AuditRestaurant"));
        }
        // 存取商品修改建議
        [HttpPost]
        public bool SaveProductAuditSuggest(Product product)
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
        // 存取商品修改建議
        [HttpPost]
        public bool SaveRestaurantAuditSuggest(Restaurant restaurant)
        {
            var query = _petsDB.Restaurants.First(p => p.RestaurantId == restaurant.RestaurantId);
            // TODO 新增欄位 AusitResult
            //query.AuditResult = restaurant.AuditResult;
            try
            {
                _petsDB.SaveChanges();
            }
            catch (Exception)
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
