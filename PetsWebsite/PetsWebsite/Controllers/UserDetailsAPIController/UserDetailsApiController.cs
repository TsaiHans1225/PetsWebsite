using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;

namespace PetsWebsite.Controllers.UserDetailsAPIController
{

    [Route("api/UserDetails/{action}")]
    [ApiController]
    public class UserDetailsApiController : ControllerBase
    {

        UserDetailsViewModel data = new UserDetailsViewModel
        {
            LastName = "Alex",
            FirstName = "Wu",
            Email = "andfwf@gmail.com",
            Phone = "22222222",
            Address = "XXXXXXXX",
            Birthday = "1995/06/11"
        };

        List<UserDetailsViewModel> datalist = new List<UserDetailsViewModel>();

        private readonly PetsDBContext _dbContext;
        public UserDetailsApiController(PetsDBContext dbContext)
        {
            _dbContext = dbContext;
            datalist.Add(data);
        }
        [HttpGet]
        public async Task<ActionResult<UserDetailsViewModel>> GetUserDetails()
        {
            int id = 1; 
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            return _dbContext.Users.Where(u => u.UserId == id).ToList().Select(u => new UserDetailsViewModel()
            {
                LastName = u.LastName,
                FirstName = u.FirstName,
                Email = u.Email,
                Phone = u.Phone,
                Address = u.Address,
                Birthday = u.Birthday?.ToShortDateString()
            }).FirstOrDefault();
        }
    }
}
