using GeoCoordinatePortable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/ClinicAPI/{action}")]
    [ApiController]
    public class ClinicAPIController : ControllerBase
    {
        private readonly PetsDBContext _PetsDB;

        public ClinicAPIController(PetsDBContext petsDB)
        {
            _PetsDB = petsDB;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetAll()
        {
            var ClinicList = _PetsDB.Clinics.ToList();
            if (_PetsDB.Clinics == null)
            {
                return NotFound();
            }
            return await _PetsDB.Clinics.ToListAsync();
        }
        [HttpGet]
        [Route("{City}/{Region?}")]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetContent(string City, string? Region)
        {
            if (Region == null)
            {
                return await _PetsDB.Clinics.Where(c => c.City == City).ToListAsync();
            }
            else
            {
                return await _PetsDB.Clinics.Where(c => c.City == City && c.Region == Region).ToListAsync();
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetInfo([FromRoute(Name = "id")] int Id)
        {
            return await _PetsDB.Clinics.Where(i => i.ClinicId == Id).ToListAsync();
        }
        [HttpGet]
        public IEnumerable<Product> GetProductInfo()
        {
            var RestList = _PetsDB.Products.ToList();
            var list = RestList.Take(10);
            return list;
        }
        [HttpGet]
        [Route("{id}")]
        public IEnumerable<RestaurantViewModel> GetNearRest([FromRoute(Name = "id")] int ClinicId)
        {
            var center = _PetsDB.Clinics.FirstOrDefault(i => i.ClinicId == ClinicId);
            if (center == null) return Enumerable.Empty<RestaurantViewModel>();
            if (!center.Longitude.HasValue && !center.Latitude.HasValue) return Enumerable.Empty<RestaurantViewModel>();
            var coord = new GeoCoordinate(center.Latitude.Value, center.Longitude.Value);
            var allRestaurants = _PetsDB.Restaurants.Where(x => x.Latitude != null && x.Longitude != null).ToList();
            var temp = allRestaurants.Select(x => new RestaurantViewModel
            {
                PhotoPath = x.PhotoPath,
                City = x.City,
                Address = x.Address,
                Phone = x.Phone,
                Region = x.Region,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                Restaurants = x.RestaurantName,
                RestaurantsId = x.RestaurantId,
                RestTime = x.BusyTime,
                describe=x.Introduction,
                dist = coord.GetDistanceTo(new GeoCoordinate(x.Latitude.Value, x.Longitude.Value))
            }).Where(x => x.dist < 10000).ToList();
            return temp;
        }
        [HttpGet]
        [Route("{Key}")]
        public async Task<ActionResult<IEnumerable<Clinic>>> Searchstr([FromRoute(Name ="Key")] string Key)
        {
            return await _PetsDB.Clinics.Where(c => c.ClinicName.Contains(Key) || c.Region.Contains(Key) || c.City.Contains(Key)||c.Address.Contains(Key)  || c.Service.Contains(Key)).ToListAsync();
        }
    }
}
