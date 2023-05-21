using HrvojeKrizan.CustomJWTAuthentication.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HrvojeKrizan.CustomJWTAuthentication.Server.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ClaimsService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            List<Claim> userClaims = new()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null) 
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach(var roleClaim in roleClaims)
                    {
                        userClaims.Add(new Claim(roleClaim.Type, roleClaim.Value));
                    }
                }
            }

            return userClaims;
        }
    }
}
