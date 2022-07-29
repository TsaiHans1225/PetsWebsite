using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class ShopController : Controller
    {
        private readonly PetsDBContext ProductDBContext;
        public ShopController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }


        //排序
        public async Task<IActionResult> Index(int id = 1)
        {

            if (id == 1)
            {
                var products =await ProductDBContext.Products
                      .OrderBy(x => x.UnitPrice)
                      .ToListAsync();
                return View(products);

               
            }
            else if (id == 2)
            {
                var products =await ProductDBContext.Products
                    .OrderByDescending(x => x.UnitPrice)
                    .ToListAsync();
                return View(products);

                
            }
            else if (id == 3)
            {
                var products = await ProductDBContext.Products
                 .Where(m => m.Species == true)
                 .ToListAsync();
                return View(products);


            }
            else if (id == 4)
            {
                var products = await ProductDBContext.Products
                 .Where(m => m.Species == false)
                 .ToListAsync();
                return View(products);
            }
            else
            {
                var products =await ProductDBContext.Products
                      .OrderBy(x => x.UnitPrice)
                      .ToListAsync();
                return View(products);

            }


        }



        //搜尋關鍵字
        [HttpPost]
        public async Task<IActionResult> Index(string txtKeyword)
        {

            var products = await ProductDBContext.Products
                    .Where(m => m.ProductName.Contains(txtKeyword))
                    .ToListAsync();

            return View(products);
        }

    }
}
