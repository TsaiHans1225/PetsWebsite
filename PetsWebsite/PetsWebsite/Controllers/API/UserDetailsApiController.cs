using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;
using System.Collections.Generic;

namespace PetsWebsite.Controllers.API
{

    [Route("api/UserDetails/{action}")]
    [ApiController]
    public class UserDetailsApiController : ControllerBase
    {
        private readonly PetsDBContext _dbContext;
        public UserDetailsApiController(PetsDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<UserDetailsViewModel>> GetUserDetails()
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            return _dbContext.Users.Where(u => u.UserId == User.GetId()).ToArray().Select(u => new UserDetailsViewModel()
            {
                LastName = u.LastName,
                FirstName = u.FirstName,
                Email = u.Email,
                Phone = u.Phone,
                City = u.City,
                Region = u.Region,
                ZipCode = u.Zipcode,
                Address = u.Address,
                Birthday = u.Birthday?.ToShortDateString()
            }).FirstOrDefault();
        }

        [HttpPost]
        public void UpdateUserDetails(UserDetailsViewModel uDetails)
        {
            var query = _dbContext.Users.FirstOrDefault(u => u.UserId == User.GetId());
            query.LastName = uDetails.LastName;
            query.FirstName = uDetails.FirstName;
            query.Email = uDetails.Email;
            query.Phone = uDetails.Phone;
            query.City = uDetails.City;
            query.Region = uDetails.Region;
            query.Zipcode = uDetails.ZipCode;
            query.Address = uDetails.Address;
            query.Birthday = Convert.ToDateTime(uDetails.Birthday);
            _dbContext.SaveChanges();
        }
    }
}
