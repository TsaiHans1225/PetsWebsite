using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly PetsDBContext ProductDBContext;


        public ShoppingCartController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }

        //加入購物車      
        public IActionResult Index()
        {
            return View();
        }
    }
}
