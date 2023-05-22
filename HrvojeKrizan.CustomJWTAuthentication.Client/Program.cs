using Blazored.LocalStorage;
using HrvojeKrizan.CustomJWTAuthentication.Client;
using HrvojeKrizan.CustomJWTAuthentication.Client.Services;
using HrvojeKrizan.CustomJWTAuthentication.Shared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace HrvojeKrizan.CustomJWTAuthentication.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient<ApplicationAuthenticationService>(client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<ApiService>(client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddAuthorizationCore(options =>
            {
                //add some policies based on claims
                options.AddPolicy(RoleClaims.SuperAdmin, policy => policy.RequireClaim(RoleClaims.ApplicationClaimType, RoleClaims.SuperAdmin));
                options.AddPolicy(RoleClaims.SpecialPermissions, policy => policy.RequireClaim(RoleClaims.ApplicationClaimType, RoleClaims.SpecialPermissions));
            });

            builder.Services.AddScoped<ApplicationAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ApplicationAuthenticationStateProvider>());

            await builder.Build().RunAsync();
        }
    }
}