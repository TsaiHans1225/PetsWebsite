using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/UserRegister/{action}")]
    [ApiController]
    public class UserRegisterAPIController : ControllerBase
    {
        private readonly PetsDBContext _PetsDB;

        public UserRegisterAPIController(PetsDBContext petsDB)
        {
            _PetsDB = petsDB;
        }
        [HttpPost]
        public async Task<bool> MenberRegister(RegisterInfo users)
        {
            var query = _PetsDB.UserAccounts.FirstOrDefault(a => a.Account == users.Account);
            if (query != null)
            {
                return false;
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
                return false;
            }
            return true;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CompanyRegister(CompanyRegisterInfo register)
        {
            bool Isregst = true;
            var query =await _PetsDB.CompanyAccounts.FirstOrDefaultAsync(a => a.Account == register.Account);
            if (query != null)
            {
                Isregst = false;
            }
            else
            {
                try
                {
                    CompanyAccount companyAccount = new CompanyAccount
                    {
                        Account = register.Account,
                        CompanyId=int.Parse(register.CompanyId),
                        Password = register.Password,
                        Company =new Company
                        {
                            CompanyId = int.Parse(register.CompanyId),
                            Email= register.Account,
                            CompanyName=register.CompanyName,
                        }
                    
                    };
                    _PetsDB.CompanyAccounts.Add(companyAccount);
                    _PetsDB.SaveChanges();
                }
                catch(Exception e)
                {
                    Isregst = false;
                }
                }
            
            return Isregst;
        }

    }
}
