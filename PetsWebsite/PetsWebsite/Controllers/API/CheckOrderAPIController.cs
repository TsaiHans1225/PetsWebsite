using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/CheckOrder/{action}")]
    [ApiController]
    public class CheckOrderAPIController : ControllerBase
    {
        private readonly PetsDBContext _dBContext;

        public CheckOrderAPIController(PetsDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        [HttpGet]
        public async Task<CustomerInfo> GetCustomerInfo()
        {
            return await _dBContext.Users.Where(c => c.UserId == User.GetId()).Select(c => new CustomerInfo()
            {
                CustomerName=c.UserName,
                Email=c.Email,
                Phone=c.Phone,
                Address=c.City+c.Region+c.Address,
            }).FirstOrDefaultAsync();
        }
        [HttpGet]
        public List<CheckOutOrderViewModle> GetPreferProductListInOrder()
        {
            return JsonConvert.DeserializeObject<List<CheckOutOrderViewModle>>(HttpContext.Session.GetString("PreOrder"));
        }

    }
}
