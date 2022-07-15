using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class PetsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
