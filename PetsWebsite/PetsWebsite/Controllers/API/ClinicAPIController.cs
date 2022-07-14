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

        public  ClinicAPIController(PetsDBContext petsDB)
        {
            _PetsDB = petsDB;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetCity()
        {
      
             return await _PetsDB.Clinics.Select(c => c.City).Distinct().ToListAsync();
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{City?}/{Region?}")]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetContent(string ?City,string? Region)
        {
            var query = _PetsDB.Clinics.Where(c => c.City== City || c.Region== Region);
            return query.ToList();
        }
    }
}
