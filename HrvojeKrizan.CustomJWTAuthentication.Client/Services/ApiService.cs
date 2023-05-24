using HrvojeKrizan.CustomJWTAuthentication.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }

        public HttpClient HttpClient
        {
            get
            {
                return _http;
            }
        }

        public async Task<WeatherForecast[]?> GetForecastAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
            }
            catch
            {
                return new WeatherForecast[0];
            }
        }
    }
}
