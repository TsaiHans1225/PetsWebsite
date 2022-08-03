using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/UserCollect/{action}")]
    [ApiController]
    public class UserCollectAPIController : ControllerBase
    {
        private readonly PetsDBContext _petsDB;

        public UserCollectAPIController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }
        [HttpGet]
        public List<int> GetUserCollectId()
        {
            return _petsDB.Collections.Where(c => c.UserId == User.GetId()).Select(c => c.ProductId).ToList();
        }
        [HttpGet]
        public List<FavoriteViewModel> GetUserCollectItem()
        {
            return _petsDB.Collections.Where(c => c.UserId == User.GetId()).Select(c => new FavoriteViewModel()
            {
                ProductId=c.ProductId,
                ProductName=c.Product.ProductName,
                Price=c.Product.UnitPrice,
                PhotoPath=c.Product.PhotoPath
            }).ToList();
        }
        [HttpGet]
        [Route("{CollecProdoctId}")]
        public bool AddCollect(int CollecProdoctId)
        {
            try
            {
                _petsDB.Collections.Add(new Collection()
                {
                    UserId = User.GetId(),
                    ProductId = CollecProdoctId,
                    CollectDate = DateTime.Now,
                });
                _petsDB.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        [HttpDelete]
        [Route("{CollecProdoctId}")]
        public bool DeleteCollect(int CollecProdoctId)
        {
            var RemoveCollect = _petsDB.Collections.FirstOrDefault(c => c.ProductId == CollecProdoctId && c.UserId == User.GetId());
            try
            {
                _petsDB.Collections.Remove(RemoveCollect);
                _petsDB.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
