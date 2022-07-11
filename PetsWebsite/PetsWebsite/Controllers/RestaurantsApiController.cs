using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/Restaurants/{action}")]
    [ApiController]
    public class RestaurantsApiController : ControllerBase
    {
        private readonly PetsDBContext _petsDBContext;
        public RestaurantsApiController(PetsDBContext petsDBContext)
        {
            _petsDBContext = petsDBContext;
        }

        // GET: api/<RestaurantsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> Restaurant()
        {
            if(_petsDBContext.Restaurants == null)
            {
                return NotFound();
            }
            return await _petsDBContext.Restaurants.ToListAsync();
        }

        //        // GET api/<RestaurantsController>/5
        //        [HttpGet("{id}")]
        //        public string Get(int id)
        //        {
        //            return "value";
        //        }

        //        // POST api/<RestaurantsController>
        //        [HttpPost]
        //        public void Post([FromBody] string value)
        //        {
        //        }

        //        // PUT api/<RestaurantsController>/5
        //        [HttpPut("{id}")]
        //        public void Put(int id, [FromBody] string value)
        //        {
        //        }

        //        // DELETE api/<RestaurantsController>/5
        //        [HttpDelete("{id}")]
        //        public void Delete(int id)
        //        {
        //        }
    }
}