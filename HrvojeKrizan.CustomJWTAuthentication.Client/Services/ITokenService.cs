using HrvojeKrizan.CustomJWTAuthentication.Shared.DTO;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public interface ITokenService
    {
        Task<TokenDTO> GetToken();
        Task RemoveToken();
        Task SetToken(TokenDTO tokenDTO);
    }
}
