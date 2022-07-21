using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;
using System.Security.Claims;

namespace PetsWebsite.Controllers.API
{
    [Route("api/UserOpt/{action}")]
    [ApiController]
    public class UserLoginAPIController : ControllerBase
    {
        private readonly PetsDBContext _PetsDB;
        public UserLoginAPIController(PetsDBContext petsDB)
        {
            _PetsDB = petsDB;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> UserLogin(Login users)
        {
            //到資料庫找資料
            var existUser = _PetsDB.UserAccounts.Join(_PetsDB.Users,
                a => a.UserId,
                u => u.UserId,
                (a, u) => new
                {
                    UserId = a.UserId,
                    Account = a.Account,
                    Password = a.Password,
                    RoleId = u.RoleId
                }).Join(_PetsDB.Roles,
                a => a.RoleId,
                r => r.RoleId,
                (a, r) =>
                new
                {
                    Account = a.Account,
                    Password = a.Password,
                    Role = r.Role1
                }
                ).FirstOrDefault(x => x.Account == users.Account && x.Password == users.Password);
            if (existUser == null)
            {
                return false;
            }
            //給予身分
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,existUser.Account),
                new Claim(ClaimTypes.Role,existUser.Role),               
            };
           
            var claimsIndntity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //建立憑證
            var claimsPrincipal = new ClaimsPrincipal(claimsIndntity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal
                );
            return true;

        }
    }
}
