using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class MemberOrderHistoriesController : Controller
    {
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}
