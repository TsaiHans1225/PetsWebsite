using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
