using System.Net.Http.Headers;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public class AuthorizationHttpMessageHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public AuthorizationHttpMessageHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.Token}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
