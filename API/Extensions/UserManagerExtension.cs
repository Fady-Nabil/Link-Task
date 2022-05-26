using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
