using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;
using PetsWebsite.Models.ViewModels;



namespace PetsWebsite.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsApiController : ControllerBase
    {
        private readonly PetsDBContext _petsDBContext;
        Restaurant rest = new Restaurant();

        public RestaurantsApiController(PetsDBContext petsDBContext)
        {
            _petsDBContext = petsDBContext;
            Console.WriteLine(this._petsDBContext);
        }

        [HttpGet]
        [Route("QryByCityRegion/{city}/rawdata")]
        public List<Restaurant> RestaurantQryByCity([FromRoute(Name = "city")] string city)
        {
            var query = (from c in _petsDBContext.Restaurants
                         where c.City == city
                         where c.State == true
                         select c).ToList<Restaurant>();

            return query;

        }

        [HttpGet]
        [Route("QryByRegion/{region}/rawdata")]
        public List<Restaurant> RestaurantQryByRegion([FromRoute(Name = "region")] string region)
        {
            var query = (_petsDBContext.Restaurants).Where(r => r.Region == region).Where(r=>r.State==true).ToList<Restaurant>();
            return query;
        }

        [HttpGet]
        [Route("QryByCityRegion/{city}/{region?}/rawdata")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> RestaurantQryCityRegion([FromRoute(Name = "city")] string? city, [FromRoute(Name = "region")] string? region)
        {
            if (region == null)
            {
                return await _petsDBContext.Restaurants.Where(c => c.City == city).ToListAsync<Restaurant>();
            }
            return await (_petsDBContext.Restaurants)
                .Where(c => c.City == city && c.Region == region).Where(c=>c.State==true).ToListAsync<Restaurant>(); ;
        }

        [HttpGet]
        [Route("Details/rawdata")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> Details()
        {

            if (_petsDBContext.Restaurants == null)
            {
                return NotFound();
            }
            return await _petsDBContext.Restaurants.Where(x=>x.State == true).ToListAsync();
        }

        [HttpGet]
        [Route("Search/rawdata")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchKey([FromQuery] string key)
        {
            return await _petsDBContext.Restaurants.Where(s => s.RestaurantName.Contains(key) || s.City.Contains(key) || s.Region.Contains(key) || s.Address.Contains(key)).Where(s=>s.State==true).ToListAsync();
        }

        [HttpGet]
        [Route("Details/rawdata/{id}")]
        public IEnumerable<RestArticlesViewModel> DetailPage([FromRoute(Name = "id")] int id)
        {
            if(_petsDBContext.Articles.Where(x=>x.RestaurantId == id).Count() == 0)
            {
                return _petsDBContext.Restaurants
                    .Where(x => x.RestaurantId == id)
                    .Where(x => x.State == true)
                    .Select(x => new RestArticlesViewModel {
                        RestID = x.RestaurantId,
                        RestName = x.RestaurantName,
                        RestPhone = x.Phone,
                        RestPhotoPath = x.PhotoPath,
                        RestIntroduction = x.Introduction,
                        RestCity = x.City,
                        RestRegion = x.Region,
                        RestAddress = x.Address,
                        RestTime = x.BusyTime,
                        RestReserve = x.Reserve,
                    });
            }
            return _petsDBContext.Articles.Include("Restaurant")
                .Where(x=>x.RestaurantId == id)
                .Where(x=>x.Restaurant.State==true)
                .Select(x=>new RestArticlesViewModel
                {
                    RestID = x.Restaurant.RestaurantId,
                    RestName = x.Restaurant.RestaurantName,
                    RestPhone = x.Restaurant.Phone,
                    RestPhotoPath = x.Restaurant.PhotoPath,
                    RestIntroduction = x.Restaurant.Introduction,
                    RestCity = x.Restaurant.City,
                    RestRegion = x.Restaurant.Region,
                    RestAddress = x.Restaurant.Address,
                    RestTime = x.Restaurant.BusyTime,
                    RestReserve = x.Restaurant.Reserve,
                    Title = x.Title,
                    Contents = x.Contents,
                    FromPlace = x.FromPlace
                });
        }

    }
}