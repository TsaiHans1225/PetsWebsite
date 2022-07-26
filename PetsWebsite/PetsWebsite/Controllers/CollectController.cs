using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{

    public class CollectController : Controller
    {
        private readonly PetsDBContext CollectDBContext;
        public CollectController(PetsDBContext _PetsDBContext)
        {
            CollectDBContext = _PetsDBContext;
        }


        //排序
        public async Task<IActionResult> Index()
        {


            var products = await CollectDBContext.Products
                  .Where(x => x.UnitPrice >= 201)
                  .ToListAsync();

            return View(products);

        }

    }
}
