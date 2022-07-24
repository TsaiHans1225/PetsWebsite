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
      
    }
}
