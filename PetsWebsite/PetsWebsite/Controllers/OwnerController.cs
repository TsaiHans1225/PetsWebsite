using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;

namespace PetsWebsite.Controllers
{
    public class OwnerController : Controller
    {
        private readonly PetsDBContext _dBContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OwnerController(PetsDBContext dBContext, IWebHostEnvironment webHostEnvironment)
        {
            _dBContext = dBContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        public IActionResult NewProduct(Product newProduct)
        {
            // 整理資料
            newProduct.CategoryId = 1;
            newProduct.CompanyId = 0;
            newProduct.ProductName = newProduct.ProductName.Trim();
            newProduct.Describe = newProduct.Describe.Trim();
            //newProduct.Discontinued = true;
            newProduct.LaunchDate = DateTime.Now;

            // 取出表單圖片及名稱
            var InputFile = HttpContext.Request.Form.Files[0]; // 在深層自動變數的地方
            var InputFilePath = "";
            if (InputFile == null)
            {
                newProduct.PhotoPath = "product_Default";
            }
            else
            {
                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                newProduct.PhotoPath = $"{newProduct.CompanyId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product", newProduct.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyToAsync(fs); // 將圖片放入指定位置
                fs.Close();
            }
            _dBContext.Products.AddAsync(newProduct);
            _dBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Route("Owner/EditProduct/{ProductId}")]
        public async Task<IActionResult> EditProduct(int ProductId)
        {
            var query = _dBContext.Products.FirstOrDefault(x => x.ProductId == ProductId);
            return View(query);
        }

        public async Task<IActionResult> SaveEditedProduct(Product editedProduct)
        {
            // 找出圖檔
            var InputFile = HttpContext.Request.Form.Files[0];
            var InputFilePath = "";
            if (InputFile == null)
            {
                editedProduct.PhotoPath = Convert.ToString(_dBContext.Products.Where(p => p.ProductId == editedProduct.ProductId).Select(p => p.PhotoPath));
            }
            else
            {
                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                editedProduct.PhotoPath = $"{editedProduct.CompanyId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product", editedProduct.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyToAsync(fs);
                fs.Close();
            }

            // 儲存資料
            var query = _dBContext.Products.FirstOrDefault(p => p.ProductId == 42);
            query.ProductName = editedProduct.ProductName.Trim();
            query.UnitPrice = editedProduct.UnitPrice;
            query.Describe = editedProduct.Describe.Trim();
            query.LaunchDate = DateTime.Now;
            query.PhotoPath = editedProduct.PhotoPath;
            await _dBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
