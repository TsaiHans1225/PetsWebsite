using System.Security.Claims;

namespace PetsWebsite.Extensions
{
    public static class ClaimExtension
    {
        public static int GetId(this ClaimsPrincipal user) 
        {
            return Convert.ToInt32(user.Claims.First(x => x.Type == "UserID").Value);
        }
        public static string GetMail(this ClaimsPrincipal user)
        {
            return user.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        }
    }
}
