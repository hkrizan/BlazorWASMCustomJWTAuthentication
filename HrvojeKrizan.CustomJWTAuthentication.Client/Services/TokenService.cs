using Blazored.LocalStorage;
using HrvojeKrizan.CustomJWTAuthentication.Shared.DTO;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService _localStorageService;

        public TokenService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetToken(TokenDTO tokenDTO)
        {
            await _localStorageService.SetItemAsync("token", tokenDTO);
        }

        public async Task<TokenDTO> GetToken()
        {
            return await _localStorageService.GetItemAsync<TokenDTO>("token");
        }

        public async Task RemoveToken()
        {
            await _localStorageService.RemoveItemAsync("token");
        }
    }
}
