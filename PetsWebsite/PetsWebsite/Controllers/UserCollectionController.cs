using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class UserCollectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
