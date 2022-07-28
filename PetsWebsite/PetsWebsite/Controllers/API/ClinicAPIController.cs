using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;


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
        public async Task<ActionResult<IEnumerable<Clinic>>> GetContent(string City,string? Region)
        {
            if (Region == null)
            {
                return await _PetsDB.Clinics.Where(c => c.City==City).ToListAsync();
            }
            else
            {
                return await _PetsDB.Clinics.Where(c => c.City == City&&c.Region == Region).ToListAsync();
            }
        }
        [HttpGet]
        [Route("{id}")]
      public async Task<ActionResult<IEnumerable<Clinic>>> GetInfo([FromRoute(Name ="id")]int Id)
        {
            return await _PetsDB.Clinics.Where(i => i.ClinicId == Id).ToListAsync();
        }
        [HttpGet]
        public IEnumerable<Restaurant> GetRestInfo()
        {
             var RestList=_PetsDB.Restaurants.ToList();
            var list = RestList.Take(10);
            return list;
        }
        public IEnumerable<Clinic> GetRestId(int id)
        {
            var center = _PetsDB.Clinics.FirstOrDefault(i => i.ClinicId == id);           
        }
    }
}
