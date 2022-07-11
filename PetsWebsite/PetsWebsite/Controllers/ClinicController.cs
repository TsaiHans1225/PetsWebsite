using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class ClinicController : Controller
    {
        public IActionResult Clinic()
        {
            return View();
        }
        public IActionResult ClinicInfo()
        {
            return View();
        }
    }
}
