using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PetsDBContext ProductDBContext;


        public CheckoutController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }

        //立即購買
        public IActionResult Index()
        {
            

            return View();
        }
    }
}
