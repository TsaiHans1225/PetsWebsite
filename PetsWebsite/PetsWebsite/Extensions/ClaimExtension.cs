using System.Security.Claims;

namespace PetsWebsite.Extensions
{
    public static class ClaimExtension
    {
        public static int GetId(this ClaimsPrincipal user) 
        {
            return Convert.ToInt32(user.Claims.First(x => x.Type == "UserID").Value);
        }
    }
}
