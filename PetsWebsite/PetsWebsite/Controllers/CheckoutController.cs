using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
