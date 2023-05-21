using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HrvojeKrizan.CustomJWTAuthentication.Server.Services
{
    public interface IJwtTokenService
    {
        JwtSecurityToken GetJwtToken(List<Claim> userClaims);
    }
}
