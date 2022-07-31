using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AuditPage()
        {
            return View();
        }
    }
}
