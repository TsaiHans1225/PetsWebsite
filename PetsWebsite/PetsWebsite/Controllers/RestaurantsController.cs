using Microsoft.AspNetCore.Mvc;

namespace 專題相關.Controllers
{
    public class RestaurantsController : Controller
    {
        public IActionResult Restaurant()
        {
            return View();
        }

        public IActionResult RestaurantDetails()
        {
            return View();
        }
    }
}
