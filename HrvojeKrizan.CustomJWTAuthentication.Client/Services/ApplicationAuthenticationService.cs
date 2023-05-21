using HrvojeKrizan.CustomJWTAuthentication.Shared.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public class ApplicationAuthenticationService
    {
        private readonly ILogger<ApplicationAuthenticationService> _logger;
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;
        private readonly ApplicationAuthenticationStateProvider _authenticationStateProvider;

        public ApplicationAuthenticationService(
            ILogger<ApplicationAuthenticationService> logger,
            HttpClient http,
            ITokenService tokenService,
            ApplicationAuthenticationStateProvider authenticationStateProvider)
        {
            _logger = logger;
            _http = http;
            _tokenService = tokenService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<RegisterUserResultDTO> RegisterUser(RegisterUserDTO request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/account/register", request);
                var result = await response.Content.ReadFromJsonAsync<RegisterUserResultDTO>();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new RegisterUserResultDTO
                {
                    Succeeded = false,
                    Errors = new List<string>()
                    {
                        "Sorry, we were unable to register you at this time. Please try again shortly."
                    }
                };
            }
        }

        public async Task<LoginUserResultDTO> LoginUser(LoginUserDTO request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/account/login", request);
                var result = await response.Content.ReadFromJsonAsync<LoginUserResultDTO>();
                await _tokenService.SetToken(result.Token);

                if (result.Succeeded)
                {
                    _authenticationStateProvider.StateChanged();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new LoginUserResultDTO
                {
                    Succeeded = false,
                    Message = "Sorry, we were unable to log you in at this time. Please try again shortly."
                };
            }
        }

        public async Task LogoutUser()
        {
            await _tokenService.RemoveToken();
            _authenticationStateProvider.StateChanged();
        }
    }
}
