using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers
{
    public class OwnerController : Controller
    {
        private readonly PetsDBContext _dBContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GoogleMapService _googleMapService;

        public OwnerController(PetsDBContext dBContext, IWebHostEnvironment webHostEnvironment, GoogleMapService googleMapService)
        {
            _dBContext = dBContext;
            _webHostEnvironment = webHostEnvironment;
            _googleMapService = googleMapService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        public IActionResult CreateRestaurant()
        {
            return View();
        }
        public IActionResult CreateClinic()
        {
            return View();
        }

        // 新增商品
        public IActionResult NewProduct(ProductViewModel newProduct)
        {
            // 整理資料
            Product product = new Product();
            product.CategoryId = 1;
            product.CompanyId = User.GetId();
            product.ProductName = newProduct.ProductName.Trim();
            product.UnitPrice = newProduct.UnitPrice;
            product.UnitsInStock = newProduct.UnitsInStock;
            product.Describe = newProduct.Describe?.Trim();
            product.State = false;
            product.LaunchDate = DateTime.Now;
            product.Discount = newProduct.Discount;
            product.Species = newProduct.Species == "0" ? true : false;

            // 取出表單圖片及名稱
            var InputFile = HttpContext.Request.Form.Files[0]; // 在深層自動變數的地方
            var InputFilePath = "";
            if (InputFile == null)
            {
                product.PhotoPath = "product_Default";
            }
            else
            {
                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                product.PhotoPath = $"{product.CompanyId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product", product.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyToAsync(fs); // 將圖片放入指定位置
                fs.Close();
            }
            _dBContext.Products.AddAsync(product);
            _dBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 帶資料跳轉修改商品頁面
        [Route("Owner/EditProduct/{ProductId}")]
        public IActionResult EditProduct(int ProductId)
        {
            var query = _dBContext.Products.First(x => x.ProductId == ProductId);
            ProductViewModel product = new ProductViewModel();
            product.ProductId = query.ProductId;
            product.ProductName = query.ProductName;
            product.UnitPrice = query.UnitPrice;
            product.UnitsInStock = query.UnitsInStock;
            product.PhotoPath = query.PhotoPath;
            product.Describe = query.Describe;
            product.Species = query.Species == true? "0" : "1";
            return View(product);
        }

        // 儲存修改商品
        public bool SaveEditedProduct(Product editedProduct)
        {
            var query = _dBContext.Products.FirstOrDefault(p => p.ProductId == editedProduct.ProductId);
            editedProduct.CompanyId = User.GetId();

            // 判斷有無修改圖檔
            if (HttpContext.Request.Form.Files.Count != 0)
            {
                // 找出舊圖路徑
                var oldPhoto = _dBContext.Products.Where(p => p.ProductId == editedProduct.ProductId).Select(p => p.PhotoPath).Single();

                // 判斷是否為null
                if (!string.IsNullOrEmpty(oldPhoto))
                {
                    var oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product", oldPhoto);
                    // 刪除舊圖
                    System.IO.File.Delete(oldPhotoPath);
                }

                // 找出圖檔
                var InputFile = HttpContext.Request.Form.Files[0];
                var InputFilePath = "";

                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                editedProduct.PhotoPath = $"{editedProduct.CompanyId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product", editedProduct.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyTo(fs);
                fs.Close();

                // 儲存圖片
                query.PhotoPath = editedProduct.PhotoPath;
            }

            // 儲存資料
            query.ProductName = editedProduct.ProductName.Trim();
            query.UnitPrice = editedProduct.UnitPrice;
            query.Describe = editedProduct.Describe.Trim();
            query.LaunchDate = DateTime.Now;
            try
            {
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 刪除商品
        [Route("Owner/DeleteProduct/{ProductId}")]
        public bool DeleteProduct(int ProductId)
        {
            var query = _dBContext.Products.First(p => p.ProductId == ProductId);
            try
            {
                var OldPhoto = query.PhotoPath;
                _dBContext.Remove(query);
                _dBContext.SaveChanges();
                if (!string.IsNullOrEmpty(OldPhoto))
                {
                    var OldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product", OldPhoto);
                    System.IO.File.Delete(OldPhotoPath);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<IActionResult> NewRestaurant(Restaurant newRestaurant)
        {
            // 整理資料
            //newRestaurant.CategoryId = 2;
            newRestaurant.CompanyId = User.GetId();
            newRestaurant.RestaurantName = newRestaurant.RestaurantName.Trim();
            newRestaurant.Address = newRestaurant.Address.Trim();
            newRestaurant.Introduction = newRestaurant.Introduction.Trim();
            newRestaurant.Reserve = newRestaurant.Reserve.Trim();
            newRestaurant.BusyTime = newRestaurant.BusyTime.Trim();
            newRestaurant.Phone = newRestaurant.Phone.Trim();
            newRestaurant.State = false;

            //GoogleMapService googleMapService = new GoogleMapService();
            var result = _googleMapService.GetLatLngByAddr($"{newRestaurant.City}{newRestaurant.Region}{newRestaurant.Address}");
            newRestaurant.Latitude = Convert.ToDouble(result.lat);
            newRestaurant.Longitude = Convert.ToDouble(result.lng);

            // 取出圖片及名稱
            var InputFile = HttpContext.Request.Form.Files[0];
            var InputFilePath = "";
            if (InputFile == null)
            {
                newRestaurant.PhotoPath = "restaurant_Default";
            }
            else
            {
                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                newRestaurant.PhotoPath = $"{newRestaurant.CompanyId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Restaurant", newRestaurant.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyTo(fs);
            }
            _dBContext.Restaurants.Add(newRestaurant);
            _dBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("Owner/EditRestaurant/{RestaurantId}")]
        public IActionResult EditRestaurant(int RestaurantId)
        {
            var query = _dBContext.Restaurants.FirstOrDefault(r => r.RestaurantId == RestaurantId);

            ViewBag.editRestaurant = query;
            return View();
        }

        public async Task<IActionResult> SaveEditedRestaurant(Restaurant editedRestaurant)
        {
            var query = _dBContext.Restaurants.FirstOrDefault(r => r.RestaurantId == editedRestaurant.RestaurantId);
            editedRestaurant.CompanyId = User.GetId();

            if (HttpContext.Request.Form.Files.Count != 0)
            {
                var oldPhoto = _dBContext.Restaurants.Where(r => r.RestaurantId == editedRestaurant.RestaurantId).Select(r => r.PhotoPath).Single();

                if (!string.IsNullOrEmpty(oldPhoto))
                {
                    var oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Restaurant", oldPhoto);
                    System.IO.File.Delete(oldPhotoPath);
                }

                var InputFile = HttpContext.Request.Form.Files[0];
                var InputFilePath = "";

                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                editedRestaurant.PhotoPath = $"{editedRestaurant.CompanyId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Restaurant", editedRestaurant.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyTo(fs);
                fs.Close();

                query.PhotoPath = editedRestaurant.PhotoPath;
            }

            // 存取地址轉經緯度
            var result = _googleMapService.GetLatLngByAddr($"{editedRestaurant.City}{editedRestaurant.Region}{editedRestaurant.Address}");

            query.RestaurantName = editedRestaurant.RestaurantName.Trim();
            query.Phone = editedRestaurant.Phone.Trim();
            query.City = editedRestaurant.City;
            query.Region = editedRestaurant.Region;
            query.Address = editedRestaurant.Address.Trim();
            query.Reserve = editedRestaurant.Reserve.Trim();
            query.BusyTime = editedRestaurant.BusyTime.Trim();
            query.Introduction = editedRestaurant.Introduction.Trim();
            query.Latitude = Convert.ToDouble(result.lat);
            query.Longitude = Convert.ToDouble(result.lng);
            await _dBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 刪除餐廳
        [Route("Owner/DeleteRestaurant/{RestaurantId}")]
        public bool DeleteRestaurant(int RestaurantId)
        {
            var query = _dBContext.Restaurants.First(r => r.RestaurantId == RestaurantId);
            try
            {
                var OldPhoto = query.PhotoPath;
                _dBContext.Remove(query);
                _dBContext.SaveChanges();
                if (!string.IsNullOrEmpty(OldPhoto))
                {
                    var OldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Restaurant", OldPhoto);
                    System.IO.File.Delete(OldPhotoPath);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //編輯診所
        public IActionResult EditClinic()
        {
            return View();
        }
    }
}
