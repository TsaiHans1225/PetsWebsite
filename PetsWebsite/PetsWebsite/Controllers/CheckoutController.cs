using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PetsDBContext _petsDB;

        public CheckoutController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }

        [Route("Checkout")]
        public IActionResult CheckOutOrder()
        {
            return View();
        }
        //立即購買
        [Route("Checkout")]
        [HttpPost]
        public IActionResult CheckOutOrder(int[]? PurProductId)
        {
            try
            {
                var PurProduct = _petsDB.ShoppingCars.Where(p => PurProductId.Contains(p.ProductId) && p.UserId == User.GetId()).Select(p => new CheckOutOrderViewModle()
                {
                    UserId = p.UserId,
                    ProductId = p.ProductId,
                    ProductName = p.Product.ProductName,
                    Count = p.Count,
                    Price = p.Product.UnitPrice,
                    PhotoPath = p.Product.PhotoPath
                }).ToList();
                HttpContext.Session.SetString("PreOrder", JsonConvert.SerializeObject(PurProduct));
                return Ok("success");
            }
            catch (Exception)
            {                
                return BadRequest("something wrong");
            }
        }
    }
}
