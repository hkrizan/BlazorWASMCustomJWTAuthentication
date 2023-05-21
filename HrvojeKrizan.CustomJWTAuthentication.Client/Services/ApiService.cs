using HrvojeKrizan.CustomJWTAuthentication.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;

        public ApiService(HttpClient http, ITokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
            try
            {
                var token = await _tokenService.GetToken();

                if (token != null && token.Expiration > DateTime.Now)
                {
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.Token}");
                }

                return await _http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
            }
            catch
            {
                return new WeatherForecast[0];
            }
        }
    }
}
