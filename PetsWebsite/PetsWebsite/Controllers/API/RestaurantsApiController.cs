using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;


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
        [Route("QryByCity/{city}/rawdata")]
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
        //規劃查詢&Region
        [HttpGet]
        [Route("QryByCityRegion/{city}/{region}/rawdata")]
        public List<Restaurant> RestaurantQryCityRegion([FromRoute(Name = "city")] string city, [FromRoute(Name = "region")] string region)
        {
            var query = (_petsDBContext.Restaurants)
                .Where(c => c.City == city && c.Region == region).ToList<Restaurant>();
            return query;
        }

        //尚未完成
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


    }
}