using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;
using PetsWebsite.Models.ViewModels;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        //規劃查詢指定City
        [HttpGet]
        [Route("QryByCityRegion/{city}/rawdata")]
        public List<Restaurant> RestaurantQryByCity([FromRoute(Name = "city")] string city)
        {
            var query = (from c in _petsDBContext.Restaurants
                         where c.City == city
                         select c).ToList<Restaurant>();

            return query;

        }
        //=============================================================================
        //規劃查詢Region
        [HttpGet]
        [Route("QryByRegion/{region}/rawdata")]
        public List<Restaurant> RestaurantQryByRegion([FromRoute(Name = "region")] string region)
        {
            var query = (_petsDBContext.Restaurants).Where(r => r.Region == region).ToList<Restaurant>();
            return query;
        }
        //=============================================================================
        //規劃查詢City&Region
        [HttpGet]
        [Route("QryByCityRegion/{city}/{region?}/rawdata")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> RestaurantQryCityRegion([FromRoute(Name = "city")] string? city, [FromRoute(Name = "region")] string? region)
        {
            if (region == null)
            {
                return await _petsDBContext.Restaurants.Where(c => c.City == city).ToListAsync<Restaurant>();
            }
            return await (_petsDBContext.Restaurants)
                .Where(c => c.City == city && c.Region == region).ToListAsync<Restaurant>(); ;
        }

        [HttpGet]
        [Route("Details/rawdata")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> Details()
        {

            if (_petsDBContext.Restaurants == null)
            {
                return NotFound();
            }
            return await _petsDBContext.Restaurants.ToListAsync();
        }

        //[HttpGet]
        //[Route("Com/rawdata")]
        //public IEnumerable<RestCommentVM> Comment([FromBody]RestCommentVM vm) 
        //{
        //    var query = _petsDBContext.Restaurants.Join(_petsDBContext.Comments,
        //        r => r.RestaurantId,
        //        c => c.CommentId,
        //        (r, c) => new RestCommentVM()
        //        {

        //           RestName = r.RestaurantName,
        //            CommTitle = c.Title,
        //            CommentContent = c.Content,
        //            CreateDate =c.SubmitTime,
        //            NowDate =c.PublicDate
        //        });
        //    return query;
        //}

        [HttpGet]
        [Route("Search/rawdata")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchKey([FromQuery] string key)
        {
            return await _petsDBContext.Restaurants.Where(s => s.RestaurantName.Contains(key) || s.City.Contains(key) || s.Region.Contains(key) || s.Address.Contains(key)).ToListAsync();
        }

        [HttpGet]
        [Route("Details/rawdata/{id}")]
        public IEnumerable<RestArticlesViewModel> DetailPage([FromRoute(Name = "id")] int id)
        {
            //return await _petsDBContext.Restaurants.Where(d => d.RestaurantId == id).ToListAsync();
            return _petsDBContext.Articles.Include("Restaurant").Where(x=>x.RestaurantId == id).Select(x=>new RestArticlesViewModel
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
                Contents = x.Contents
            });
        }

    }
}