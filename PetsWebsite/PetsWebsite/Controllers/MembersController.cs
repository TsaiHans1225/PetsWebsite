using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers
{
    public class MembersController : Controller
    {
        private readonly PetsDBContext _PetsDB;
        public MembersController(PetsDBContext _PetsDB)
        {
            this._PetsDB = _PetsDB;
        }

        [HttpPost]
        public IActionResult Register(RegisterInfo users)
        {
            var query = _PetsDB.UserLogins.FirstOrDefault(a => a.Account == users.Account);
            if (query != null)
            {
                return Redirect("/Home/Index");
            }
            UserLogin userAccount = new UserLogin
            {
                Account = users.Account,
                Password = users.Password,
                User = new User
                {
                    LastName = users.LastName,
                    FirstName = users.FirstName,
                    Email = users.Account,
                    RoleId = 1
                }
            };

            try
            {
                _PetsDB.UserLogins.Add(userAccount);
                _PetsDB.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Redirect("/Home/Index");

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Home/Index");
        }
        public IActionResult FacebookLogin()
        {
            var auth = new AuthenticationProperties()
            {
                RedirectUri = "Members/FacebookResponse"
            };
            return Challenge(auth, FacebookDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> FacebookResponse()
        {
            var data = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var temp = data.Principal.Claims.Select(x => new
            {
                x.Type,
                x.Value,
                x.ValueType,
                x.Issuer,
                x.OriginalIssuer,
                x.Properties
            });
            var LoginProvider = temp.Select(c => c.Issuer).First();
            var Providerkey = temp.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;

            return Redirect("/Home/Index");
        }
    }
}
