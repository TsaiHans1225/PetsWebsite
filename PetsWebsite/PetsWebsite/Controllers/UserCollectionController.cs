using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class UserCollectionController : Controller
    {
        private readonly PetsDBContext _petsDB;
        public UserCollectionController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _petsDB.Products
                      .OrderBy(x => x.UnitPrice).Where(p => p.State == true)
                      .ToListAsync();

            return View(products);
        }
    }
}
