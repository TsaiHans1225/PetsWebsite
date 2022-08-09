using Microsoft.AspNetCore.Mvc;

namespace PetsWebsite.Controllers
{
    public class ResetPwdController : Controller
    {
        private readonly IConfiguration _config;
        public ResetPwdController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult ResetPwdIndex(string verify)
        {
            // 由信件連結回來會帶著參數verify
            if (string.IsNullOrEmpty(verify))
            {
                ViewData["ErrorMsg"] = "缺少驗證碼";
                return View();
            }
            // 取得系統自訂金鑰
            string SecretKey = _config.GetValue<string>("SecretKey");

            try
            {
                // 使用 3DES 解密驗證碼
                //TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                //MD5 md5 = new MD5CryptoServiceProvider();
                //byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
                //byte[] md5result = md5.ComputeHash(buf);
                //string md5Key = BitConverter.ToString(md5result).Replace("-", "").ToLower().Substring(0, 24);
                //DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
                //DES.Mode = CipherMode.ECB;
                //DES.Padding = PaddingMode.PKCS7;
                //ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                //byte[] Buffer = Convert.FromBase64String(verify);
                //string deCode = UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

                //verify = deCode; //解密後還原資料
            }
            catch (Exception ex)
            {
                ViewData["ErrorMsg"] = "驗證碼錯誤";
                return View();
            }

            // 取出帳號
            string email = verify.Split('|')[0];

            // 取得重設時間
            string ResetTime = verify.Split('|')[1];

            // 檢查時間是否超過30分鐘
            DateTime dResetTime = Convert.ToDateTime(ResetTime);
            TimeSpan TS = new TimeSpan(DateTime.Now.Ticks - dResetTime.Ticks);
            double diff = Convert.ToDouble(TS.TotalMinutes);
            if(diff > 30)
            {
                ViewData["ErrorMsg"] = "超過驗證有效時間，請重寄驗證碼";
                return View();
            }

            // 驗證碼成功，加入Session
            HttpContext.Session.SetString("ResetPwdEmail", email);
            return View();
        }

        //廠商忘記密碼View
        public IActionResult ResetCmpPwdIndex(string verify)
        {
            // 由信件連結回來會帶著參數verify
            if (string.IsNullOrEmpty(verify))
            {
                ViewData["ErrorMsg"] = "缺少驗證碼";
                return View();
            }
            // 取得系統自訂金鑰
            string SecretKey = _config.GetValue<string>("SecretKey");

            try
            {
                // 使用 3DES 解密驗證碼
                //TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                //MD5 md5 = new MD5CryptoServiceProvider();
                //byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
                //byte[] md5result = md5.ComputeHash(buf);
                //string md5Key = BitConverter.ToString(md5result).Replace("-", "").ToLower().Substring(0, 24);
                //DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
                //DES.Mode = CipherMode.ECB;
                //DES.Padding = PaddingMode.PKCS7;
                //ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                //byte[] Buffer = Convert.FromBase64String(verify);
                //string deCode = UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

                //verify = deCode; //解密後還原資料
            }
            catch (Exception ex)
            {
                ViewData["ErrorMsg"] = "驗證碼錯誤";
                return View();
            }

            // 取出帳號
            string email = verify.Split('|')[0];

            // 取得重設時間
            string ResetTime = verify.Split('|')[1];

            // 檢查時間是否超過30分鐘
            DateTime dResetTime = Convert.ToDateTime(ResetTime);
            TimeSpan TS = new TimeSpan(DateTime.Now.Ticks - dResetTime.Ticks);
            double diff = Convert.ToDouble(TS.TotalMinutes);
            if (diff > 30)
            {
                ViewData["ErrorMsg"] = "超過驗證有效時間，請重寄驗證碼";
                return View();
            }

            // 驗證碼成功，加入Session
            HttpContext.Session.SetString("ResetComPwdEmail", email);
            return View();
        }
    }
}
