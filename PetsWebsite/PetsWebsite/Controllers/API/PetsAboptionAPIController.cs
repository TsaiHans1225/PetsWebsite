using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace PetsWebsite.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsAboptionAPIController : ControllerBase
    {
        private readonly PetsAboptionResource _petsAboptionResource;
        private readonly IMemoryCache _cache;
        public PetsAboptionAPIController(PetsAboptionResource petsAboptionResource, IMemoryCache cache)
        {
            _petsAboptionResource = petsAboptionResource;
            _cache = cache;
        }

        HttpClient client = new HttpClient();
        [HttpGet]
        [Route("GetCount")]
        public async Task<int> GetCount()
        {

            client.BaseAddress = new Uri(_petsAboptionResource.pets);
            string jsonString = await client.GetStringAsync("");
            List<PetsAboption> data = JsonConvert.DeserializeObject<List<PetsAboption>>(jsonString);
            return data.Count();

        }
        [HttpGet]
        [Route("GetPage")]
        public async Task<IEnumerable<PetsAboption>> GetPage([FromQuery] int curPage, int showPage)
        {
            client.BaseAddress = new Uri(_petsAboptionResource.pets);
            string jsonString = await client.GetStringAsync("");
            List<PetsAboption> data = JsonConvert.DeserializeObject<List<PetsAboption>>(jsonString);
            var query = data.OrderBy(x => x.animal_id).Skip((curPage - 1) * showPage).Take(showPage).ToList();
            object CacheData;
            MemoryCacheEntryOptions memoryCache = new MemoryCacheEntryOptions();
            memoryCache.SetPriority(CacheItemPriority.Normal);
            memoryCache.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            _cache.Set("CacheByQuery", query, memoryCache);
            if (_cache.TryGetValue("CacheByQuery", out CacheData))
            {
                return query = CacheData as List<PetsAboption>;
            }
            return Enumerable.Empty<PetsAboption>();
        }


    }
}
