using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

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
        public async Task<bool> AddShopCar(int ProductId)
        {
            var user = _PetsDB.Users.Include("ShoppingCars")
                .FirstOrDefault(x=>x.UserId == User.GetId());
            if (user == null)
            {
                return false;
            }
            var exist = user.ShoppingCars.FirstOrDefault(s => s.ProductId == ProductId);
            if (exist != null)
            {
                exist.Count+=1;
            }
            else
            {
                user.ShoppingCars.Add(new ShoppingCar() { ProductId = ProductId ,Count=1});
            }
            _PetsDB.SaveChanges();
            return true;
        }
        [HttpGet]
        [Authorize]
        public List<ShopcarViewModel> GetShopCarList()
        {
            return _PetsDB.ShoppingCars.Where(x => x.UserId == User.GetId()).Select(x=>new ShopcarViewModel
            {
                ProductName= x.Product.ProductName,
                PicturePath=x.Product.PhotoPath,
                Count=x.Count,
                Price=x.Product.UnitPrice,
                ShopCarId=x.ProductId,
            }).ToList();

        }
        [HttpDelete]
        [Authorize]
        [Route("{ProductId}")]
        public bool RemoveShopCar(int ProductId)
        {
            var user = _PetsDB.Users.Include("ShoppingCars")
                .FirstOrDefault(x => x.UserId == User.GetId());
            var exist = _PetsDB.ShoppingCars.FirstOrDefault(x => x.ProductId == ProductId && x.UserId == User.GetId());
            if (exist != null)
            {
                user.ShoppingCars.Remove(exist);
                _PetsDB.SaveChanges();
                return true;
            }
            return false;
        }
        [HttpPut]
        [Authorize]
        public bool ChangeShopCarCount(MemShopCarCount memShopCarCount)
        {
            var exist = _PetsDB.ShoppingCars.FirstOrDefault(x => x.ProductId == memShopCarCount.ProductId && x.UserId == User.GetId());
            if (exist != null)
            {
                exist.Count= memShopCarCount.Count;
                _PetsDB.SaveChanges();
                return true;
            }
            return false;
        }
    }


}
