using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;

        public ApplicationAuthenticationStateProvider(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetToken();
            var identity = string.IsNullOrEmpty(token?.Token) || token?.Expiration < DateTime.Now
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token.Token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            string json = Encoding.UTF8.GetString(jsonBytes);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
