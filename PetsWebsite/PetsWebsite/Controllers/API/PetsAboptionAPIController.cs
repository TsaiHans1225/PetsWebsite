using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsAboptionAPIController : ControllerBase
    {
        private readonly PetsAboptionResource _petsAboptionResource;
        public PetsAboptionAPIController(PetsAboptionResource petsAboptionResource)
        {
            _petsAboptionResource = petsAboptionResource;
        }

        [HttpGet]
        [Route("GetPets")]
        public ActionResult<IEnumerable<PetsAboption>> GetPets()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_petsAboptionResource.pets);
            string jsonString = client.GetStringAsync("").GetAwaiter().GetResult();
            List<PetsAboption> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PetsAboption>>(jsonString);
           
            return data;

        }
    }
}
