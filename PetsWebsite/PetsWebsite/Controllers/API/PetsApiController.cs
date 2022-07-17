using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;
using System.Linq;

namespace PetsWebsite.Controllers.API
{
    [Route("api/PetsApi/{action}")]
    [ApiController]
    public class PetsApiController : ControllerBase
    {
        private readonly PetsWebsite.Models.PetsDBContext _petsDBContext;

        public PetsApiController(PetsDBContext petsDBContext)
        {
            _petsDBContext = petsDBContext;
        }

        [HttpGet]
        public IQueryable<PetsViewModel> GetPets()
        {
            int id = 5;
            var query = _petsDBContext.Pets.Where(p => p.UserId == id).Select(p => new PetsViewModel
            {
                PetName = p.PetName,
                Species = p.Species,
                Gender = p.Gender == false? "母":"公",
                Age = p.Age
            });

            return query;
        }

        [HttpPost]
        public void CreatePet(PetsViewModel newPet)
        {
            int id = 5;
            Pet addpet = new Pet()
            {
                UserId = id,
                PetName = newPet.PetName,
                Species = newPet.Species,
                Gender = newPet.Gender == "0" ? false : true,
                Age = newPet.Age
            };

            try
            {
                _petsDBContext.Pets.Add(addpet);
                _petsDBContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
