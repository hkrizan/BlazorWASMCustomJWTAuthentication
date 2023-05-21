using HrvojeKrizan.CustomJWTAuthentication.Data.Models;
using System.Security.Claims;

namespace HrvojeKrizan.CustomJWTAuthentication.Server.Services
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
    }
}
