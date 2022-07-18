using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers.API
{
    [Route("api/ClinicAPI/{action}/{City?}")]
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
            if (_PetsDB.Clinics == null)
            {
                return NotFound();
            }
            return await _PetsDB.Clinics.ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetCity()
        {

            return await _PetsDB.Clinics.Select(c => c.City).Distinct().ToListAsync();
        }

        [HttpGet]
        //[Route("api/ClinicAPI/[action]")]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetContent(string City)
        {
             
            return await _PetsDB.Clinics.Where(c => c.City== City).ToListAsync();
        }
    }
}
