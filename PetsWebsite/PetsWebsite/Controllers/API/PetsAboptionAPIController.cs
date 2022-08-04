using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using PetsWebsite.Models.ViewModels;

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
        [HttpGet]
        [Route("GetPage")]
        public async Task<PetAdoptViewModel> GetPage([FromQuery] int curPage, int showPage, string? kinds, string? key)
        {
            HttpClient client = new HttpClient();
            if (!_cache.TryGetValue("CacheByQuery", out List<PetsAboption> CacheData))
            {
                client.BaseAddress = new Uri(_petsAboptionResource.pets);
                string jsonString = await client.GetStringAsync("");
                CacheData = JsonConvert.DeserializeObject<List<PetsAboption>>(jsonString);
                //Cache
                MemoryCacheEntryOptions memoryCache = new MemoryCacheEntryOptions();
                memoryCache.SetPriority(CacheItemPriority.Normal);
                memoryCache.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set("CacheByQuery", CacheData, memoryCache);
            }

            return new PetAdoptViewModel() {
                PetsAdoptions = CacheData.OrderBy(x => x.animal_id)
                .Where(x => { return kinds == null ? true : x.animal_kind == kinds; })
                .Where(k => { return key == null ? true : k.animal_Variety.Contains(key) || k.animal_kind.Contains(key) || k.animal_foundplace.Contains(key) || k.animal_place.Contains(key); }).Skip((curPage - 1) * showPage).Take(showPage).ToList(),
                Count = CacheData.OrderBy(x => x.animal_id)
                .Where(x => { return kinds == null ? true : x.animal_kind == kinds; })
                .Where(k => { return key == null ? true : k.animal_Variety.Contains(key) || k.animal_kind.Contains(key) || k.animal_foundplace.Contains(key) || k.animal_place.Contains(key); }).Count()
            };
        }
    }
}
