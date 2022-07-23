using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers.API
{
    [Route("api/ShoppingCar/{action}")]
    [ApiController]
    public class ShoppingCarAPIController : ControllerBase
    {
        private readonly PetsDBContext _PetsDB;

        public ShoppingCarAPIController(PetsDBContext petsDB)
        {
            _PetsDB = petsDB;
        }

        [HttpGet]
        [Authorize]
        [Route("{ProductId}")]
        public async Task<bool> GetShopCar(int ProductId)
        {
            var user = _PetsDB.Users.Find(User.GetId());
            if (user == null)
            {
                return false;
            }
            user.ShoppingCars.Add(new ShoppingCar() { ProductId = ProductId });
            _PetsDB.SaveChanges();
            return true;
        }
    }
}
