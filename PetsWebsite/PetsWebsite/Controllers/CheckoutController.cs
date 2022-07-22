using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PetsDBContext ProductDBContext;


        public CheckoutController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }

        //立即購買
        public async Task<IActionResult> Index(int id)
        {
            var products = await ProductDBContext.Products
                .Where(m => m.ProductId == id)
                .ToListAsync();

            return View(products);
        }
    }
}
