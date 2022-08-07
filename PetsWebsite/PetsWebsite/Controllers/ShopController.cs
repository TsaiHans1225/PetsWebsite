using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers
{
    public class ShopController : Controller
    {
        private readonly PetsDBContext ProductDBContext;
        public ShopController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }



        public async Task<IActionResult> filter(int id = 1)
        {

            if (id == 1)
            {
                var products = await ProductDBContext.Products
                      .OrderBy(x => x.UnitPrice).Where(p => p.State == true)
                      .ToListAsync();
                return View(products);


            }
            else if (id == 2)
            {
                var products = await ProductDBContext.Products
                    .OrderByDescending(x => x.UnitPrice).Where(p => p.State == true)
                    .ToListAsync();
                return View(products);


            }
            else if (id == 3)
            {
                var products = await ProductDBContext.Products
                 .Where(m => m.Species == true).Where(p => p.State == true)
                 .ToListAsync();
                return View(products);


            }
            else if (id == 4)
            {
                var products = await ProductDBContext.Products
                 .Where(m => m.Species == false).Where(p => p.State == true)
                 .ToListAsync();
                return View(products);
            }
            else
            {
                var products = await ProductDBContext.Products
                      .OrderBy(x => x.UnitPrice).Where(p => p.State == true)
                      .ToListAsync();
                return View(products);

            }


        }
        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var products = await ProductDBContext.Products.Where(p => p.State == true)
                      .OrderByDescending(x => x.UnitPrice)
                      .ToListAsync();

            int pageSize = 8;

            return View(ShopPagedList<Product>.Create(products, pageNumber ?? 1, pageSize));
        }



        //搜尋關鍵字
        [HttpPost]
        public async Task<IActionResult> ProductKeyword(string txtKeyword)
        {

            var products = await ProductDBContext.Products
                    .Where(m => m.ProductName.Contains(txtKeyword)).Where(p => p.State == true)
                    .ToListAsync();

            return View(products);
        }

    }
}
