using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class MedicalController : Controller
    {
        public IActionResult Medical()
        {
            return View();
        }
        public IActionResult MedicalInfo()
        {
            return View();
        }
    }
}
