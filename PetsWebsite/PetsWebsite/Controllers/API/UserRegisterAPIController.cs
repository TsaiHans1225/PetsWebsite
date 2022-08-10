﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PetsWebsite.Controllers.API
{
    [Route("api/UserRegister/{action}")]
    [ApiController]
    public class UserRegisterAPIController : ControllerBase
    {
        private readonly PetsDBContext _PetsDB;
        private readonly IConfiguration _configuration;

        public UserRegisterAPIController(PetsDBContext petsDB, IConfiguration configuration)
        {
            _PetsDB = petsDB;
            _configuration = configuration;
        }
        //會員註冊
        [HttpPost]
        public async Task<bool> MenberRegister(RegisterInfo users)
        {
            var query = _PetsDB.UserLogins.Where(a => a.User.Email == users.Account).ToList();
            if (!query.Any())
            {
                //將密碼使用 SHA256 雜湊運算(不可逆)
                string salt = users.Account.Split("@")[0]; //使用信箱@符號前面字串當作密碼鹽
                SHA256 sha256 = SHA256.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(salt + users.Password); //將密碼鹽及密碼組合
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("X2"));
                }
                string NewPwd = result.ToString(); // 雜湊運算後密碼

                UserLogin userAccount = new UserLogin
                {
                    LoginProvider = "cookies",
                    ProviderKey = users.Account,
                    Verification = false,
                    User = new User
                    {
                        LastName = users.LastName,
                        FirstName = users.FirstName,
                        Email = users.Account,
                        RoleId = 1,
                        Password = NewPwd,
                    }
                };
                try
                {
                    _PetsDB.UserLogins.Add(userAccount);
                    _PetsDB.SaveChanges();
                    // 寄送驗證信
                    if (users.Account != null)
                    {
                        // 取出驗證信箱
                        string verifyEmail = users.Account;
                        // 取出寄信系統金鑰
                        string SecretKey = _configuration.GetValue<string>("Email:SecretKey");
                        // 產生帳號+時間驗證碼
                        string sVerify = $"{verifyEmail} | {DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                        // ....
                        // 將加密後的密碼使用網址編碼處理
                        sVerify = HttpUtility.UrlEncode(sVerify);
                        // 網站網址
                        string webPath = "https://pet.tgm101.club/";
                        //string webPath = "https://localhost:5500/";
                        // 從信件連結傳回verify給後端做處理後導頁
                        string receivePage = "Members/GetGoogleVerify";
                        // 信件內容範本
                        string mailContent = "點擊以下連結以驗證信箱，驗證後請返回本網站進行登入。謝謝。<br><br>";
                        mailContent = $"{mailContent}<a href={webPath}{receivePage}?verify={sVerify}>點此連結驗證信箱</a>";
                        // 信件title
                        string mailSubject = "EVERYPETS網站驗證信";

                        // google發信帳號密碼
                        string GoogleMailUserId = _configuration.GetValue<string>("Email:MailUserID");
                        string GoogleMailUserPassword = _configuration.GetValue<string>("Email:MailUserPwd");

                        // 使用google mail server發信
                        string SmtpServer = _configuration.GetValue<string>("Email:SmtpServer");
                        int SmtpPort = _configuration.GetValue<int>("Email:SmtpPort");
                        MailMessage mms = new MailMessage();
                        mms.From = new MailAddress(GoogleMailUserId);
                        mms.Subject = mailSubject;
                        mms.Body = mailContent;
                        mms.IsBodyHtml = true;
                        mms.SubjectEncoding = Encoding.UTF8;
                        mms.To.Add(new MailAddress(verifyEmail));
                        using (SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.Credentials = new NetworkCredential(GoogleMailUserId, GoogleMailUserPassword); // 寄信帳密
                            smtpClient.Send(mms);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                return true;
            }
            else if (query.FirstOrDefault(o => o.LoginProvider != "cookies" ? o.LoginProvider == "cookies" : false ) == null && query.FirstOrDefault(o => o.LoginProvider == "cookies") == null)
            {
                var UserInfo = _PetsDB.Users.FirstOrDefault(u => u.Email == users.Account);
                UserInfo.Password = users.Password;
                UserInfo.Account = users.Account;
                UserLogin CookiesLogin = new UserLogin()
                {
                    UserId = UserInfo.UserId,
                    LoginProvider = "cookies",
                    ProviderKey = users.Account,
                };
                _PetsDB.UserLogins.Add(CookiesLogin);
                _PetsDB.SaveChanges();
                return true;
            }
            return false;

        }

        //廠商註冊
        [HttpPost]
        public async Task<ActionResult<bool>> CompanyRegister(CompanyRegisterInfo register)
        {
            bool Isregst = true;
            var query = await _PetsDB.CompanyAccounts.FirstOrDefaultAsync(a => a.Account == register.Account);
            if (query != null)
            {
                Isregst = false;
            }
            else
            {
                try
                {
                    //將密碼使用 SHA256 雜湊運算(不可逆)
                    string salt = register.Account.Split('@')[0]; //使用信箱@符號前面字串當作密碼鹽
                    SHA256 sha256 = SHA256.Create();
                    byte[] bytes = Encoding.UTF8.GetBytes(salt + register.Password); //將密碼鹽及密碼組合
                    byte[] hash = sha256.ComputeHash(bytes);
                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        result.Append(hash[i].ToString("X2"));
                    }
                    string NewPwd = result.ToString();

                    CompanyAccount companyAccount = new CompanyAccount
                    {
                        Account = register.Account,
                        CompanyId = int.Parse(register.CompanyId),
                        Password = NewPwd,
                        Company = new Company
                        {
                            CompanyId = int.Parse(register.CompanyId),
                            Email = register.Account,
                            CompanyName = register.CompanyName,
                        }


                    };
                    _PetsDB.CompanyAccounts.Add(companyAccount);
                    _PetsDB.SaveChanges();
                }
                catch (Exception e)
                {
                    Isregst = false;
                }
            }

            return Isregst;
        }
    }
}

