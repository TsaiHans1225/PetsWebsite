using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace 專題相關.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly PetsDBContext _dBContext;
        public RestaurantsController(PetsDBContext dBContext)
        {
            _dBContext = dBContext;
        }


        public  async Task<IActionResult> Restaurant()
        {
            if ( _dBContext.Restaurants == null)
            {
                return NotFound();
            }
            return View();
        }

        //[HttpPost]
        //public IActionResult Restaurant()
        //{
           
        //    return View();
        //}

        //public IActionResult RestaurantDetails()
        //{
        //    return View();
        //}
    }
}
