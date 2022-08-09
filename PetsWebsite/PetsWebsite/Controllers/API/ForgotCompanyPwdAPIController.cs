using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PetsWebsite.Controllers.API
{
    [Route("api/ForgotCmpPwd/{action}")]
    [ApiController]
    public class ForgotCompanyPwdAPIController : ControllerBase
    {
        private readonly PetsDBContext _dbContext;
        private readonly IConfiguration _configuration;

        public ForgotCompanyPwdAPIController(PetsDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("{email}")]
        public bool SendMailToken(string email)
        {
            // 檢查來源
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // 檢查資料庫是否有這個帳號
            var existUser = _dbContext.CompanyAccounts.FirstOrDefault(u => u.Account == email);

            if (existUser != null)
            {
                // 取出會員信箱
                string UserEmail = existUser.Account;

                // 取出系統自訂金鑰 => 在appsettings.json設定
                string SecretKey = _configuration.GetValue<string>("Email:SecretKey");

                // 產生帳號+時間驗證碼
                string sVerify = $"{email} | {DateTime.Now:yyyy/MM/dd HH:mm:ss}";

                //// 將驗證碼使用 3DES 加密
                //TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                //MD5 md5 = new MD5CryptoServiceProvider();
                //byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
                //byte[] result = md5.ComputeHash(buf);
                //string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
                //DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
                //DES.Mode = CipherMode.ECB;
                //ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                //byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
                //sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)); // 3DES 加密後驗證碼

                // 將加密後的密碼使用網址編碼處理
                sVerify = HttpUtility.UrlEncode(sVerify);

                // 網站網址
                string webPath = "https://pet.tgm101.club/";
                //string webPath = "https://localhost:5500/";

                // 從信件連結回到重設密碼頁面
                string receivePage = "ResetPwd/ResetCmpPwdIndex";

                // 信件內容範本
                string mailContnet = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
                mailContnet = $"{mailContnet}<a href={webPath}{receivePage}?verify={sVerify}>點此連結</a>";

                // 信件主題
                string mailSubject = "[test]重設密碼申請信";

                // Google發信帳號密碼
                string GoogleMailUserID = _configuration.GetValue<string>("Email:MailUserID");
                string GoogleMailUserPassword = _configuration.GetValue<string>("Email:MailUserPwd");

                // 使用google mail Server發信
                string SmtpServer = _configuration.GetValue<string>("Email:SmtpServer");
                int SmtpPort = _configuration.GetValue<int>("Email:SmtpPort");
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(GoogleMailUserID);
                mms.Subject = mailSubject;
                mms.Body = mailContnet;
                mms.IsBodyHtml = true;
                mms.SubjectEncoding = Encoding.UTF8;
                mms.To.Add(new MailAddress(UserEmail));

                using (SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPassword); // 寄信帳密
                    smtpClient.Send(mms);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public string DoResetPwd(string newPwd, string confirmPwd)
        {
            //DoResetPwdOut outModel = new DoResetPwdOut();

            //// 檢查是否有輸入密碼
            //if (string.IsNullOrEmpty(inModel.NewUserPwd))
            //{
            //	outModel.ErrMsg = "請輸入新密碼";
            //	return Json(outModel);
            //}
            //if (string.IsNullOrEmpty(inModel.CheckUserPwd))
            //{
            //	outModel.ErrMsg = "請輸入確認新密碼";
            //	return Json(outModel);
            //}
            //if (inModel.NewUserPwd != inModel.CheckUserPwd)
            //{
            //	outModel.ErrMsg = "新密碼與確認新密碼不相同";
            //	return Json(outModel);
            //}
            if (newPwd != null && confirmPwd != null)
            {
                var userEmail = HttpContext.Session.GetString("ResetComPwdEmail");
                if (userEmail == null || userEmail.Length == 0)
                {
                    //outModel.ErrMsg = "無修改帳號";
                    //return Json(outModel);
                    return "無修改帳號";
                }
                else
                {
                    //將新密碼使用 SHA256 雜湊運算(不可逆)
                    string salt = userEmail.Split("@")[0]; //使用信箱@符號前面字串當作密碼鹽
                    SHA256 sha256 = SHA256.Create();
                    byte[] bytes = Encoding.UTF8.GetBytes(salt + newPwd); //將密碼鹽及新密碼組合
                    byte[] hash = sha256.ComputeHash(bytes);
                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        result.Append(hash[i].ToString("X2"));
                    }
                    string NewPwd = result.ToString(); // 雜湊運算後密碼
                    var query = _dbContext.CompanyAccounts.FirstOrDefault(u => u.Account == userEmail);
                    if (query != null)
                    {
                        query.Password = NewPwd;
                        _dbContext.SaveChanges();
                        return "已存取新密碼";
                    }
                    else
                    {
                        return "未找到該帳號";
                    }
                }
            }
            else
            {
                return "你到底想不想改密碼?";
            }
        }
    }
}
