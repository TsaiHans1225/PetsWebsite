using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
