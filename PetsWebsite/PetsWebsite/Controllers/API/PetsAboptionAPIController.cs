using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

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
        public async Task<IEnumerable<PetsAboption>> GetPage([FromQuery] int curPage, int showPage, string? kinds, string? key)
        {
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
            return CacheData.OrderBy(x => x.animal_id)
                .Where(x => { return kinds == null ? true : x.animal_kind == kinds; })
                .Where(k => { return key == null ? true : k.animal_Variety.Contains(key) || k.animal_kind.Contains(key) || k.animal_foundplace.Contains(key) || k.animal_place.Contains(key); }).Skip((curPage - 1) * showPage).Take(showPage).ToList();
        }
        [HttpGet]
        [Route("GetKeyWord")]
        public async Task<IEnumerable<PetsAboption>> GetKeyWord([FromQuery]string? key)
        {
            if(!_cache.TryGetValue("CacheByKey",out List<PetsAboption> data))
            {
                client.BaseAddress = new Uri(_petsAboptionResource.pets);
                string jsonString = await client.GetStringAsync("");
                data = JsonConvert.DeserializeObject<List<PetsAboption>>(jsonString);

                MemoryCacheEntryOptions cache = new MemoryCacheEntryOptions();
                cache.SetPriority(CacheItemPriority.Normal);
                cache.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set("CacheByKey", data, cache);
            }
            return data.Where(k => k.animal_Variety.Contains(key) || k.animal_kind.Contains(key) || k.animal_foundplace.Contains(key) || k.animal_place.Contains(key));
        }
    }
}
