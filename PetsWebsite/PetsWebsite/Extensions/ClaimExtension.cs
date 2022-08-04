using System.Security.Claims;

namespace PetsWebsite.Extensions
{
    public static class ClaimExtension
    {
        //取得UserID
        public static int GetId(this ClaimsPrincipal user) 
        {
            return Convert.ToInt32(user.Claims.First(x => x.Type == "UserID").Value);
        }
        //取得UserEmail
        public static string GetMail(this ClaimsPrincipal user)
        {
            return user.Claims.First(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}
