using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
        //會員登入
        [HttpPost]
        public async Task<int> UserLogin(Login users)
        {
            //將新密碼使用 SHA256 雜湊運算(不可逆)
            string salt = users.Account.Split("@")[0]; //使用p當作密碼鹽
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(salt + users.Password); //將密碼鹽及新密碼組合
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            string NewPwd = result.ToString(); // 雜湊運算後密碼

            //到資料庫找資料
            var existUser =await _PetsDB.UserLogins.Join(_PetsDB.Users,
                a => a.UserId,
                u => u.UserId,
                (a, u) => new
                {
                    UserId = a.UserId,
                    Account = a.ProviderKey,
                    Password = u.Password,
                    RoleId = u.RoleId,
                    Verification = a.Verification,
                    Email=u.Email,
                }).Join(_PetsDB.Roles,
                a => a.RoleId,
                r => r.RoleId,
                (a, r) =>
                new
                {
                    UserId=a.UserId,
                    Account = a.Account,
                    Password = a.Password,
                    Role = r.Role1,
                    Verification = a.Verification,
                    Email = a.Email,
                }
                ).FirstOrDefaultAsync(x => x.Account == users.Account && x.Password == NewPwd);
            if (existUser == null)
            {
                return 1;
            }
            else if (existUser.Verification == false)
            {
                return 2;
            }
            //給予身分
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,existUser.Account),
                new Claim(ClaimTypes.Role,existUser.Role),
                new Claim("UserID",existUser.UserId.ToString()),
                new Claim(ClaimTypes.Email,existUser.Email),
            };
           
            var claimsIndntity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //建立憑證
            var claimsPrincipal = new ClaimsPrincipal(claimsIndntity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal
                );
            return 3;

        }
        //廠商登入
        [HttpPost]
        public async Task<bool> CompanyLogin(Login users)
        {
            string salt = users.Account.Split("@")[0];
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(salt + users.Password);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for(int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            string Password = result.ToString();
            //到資料庫找資料
            var existUser = await _PetsDB.CompanyAccounts.FirstOrDefaultAsync(x => x.Account == users.Account && x.Password == Password);
            if (existUser == null)
            {
                return false;
            }
            //給予身分
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,existUser.Account),
                new Claim(ClaimTypes.Role,"Company"),
                new Claim("UserID",existUser.CompanyId.ToString())
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
