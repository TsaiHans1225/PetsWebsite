using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class OwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }
    }
}
