using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class UserDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
