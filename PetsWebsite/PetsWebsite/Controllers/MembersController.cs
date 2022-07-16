using Microsoft.AspNetCore.Authentication;
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
            var query = _PetsDB.UserAccounts.FirstOrDefault(a => a.Account == users.Account);
            if (query != null)
            {
                return Redirect("/Home/Index");
            }
            UserAccount userAccount = new UserAccount
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
                _PetsDB.UserAccounts.Add(userAccount);
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
    }
}
