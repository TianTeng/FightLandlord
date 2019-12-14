using System.Net.WebSockets;
using BetGame.DDZ.WasmClient.Services;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BetGame.DDZ.WasmClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ApiService>();
            services.AddScoped<FunctionHelper>();
            services.AddScoped<LocalStorage>();
            services.AddScoped<CustomAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthStateProvider>());
            services.AddAuthorizationCore();
            //services.AddScoped(sp=>new ClientWebSocket());
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            WebAssemblyHttpMessageHandlerOptions.DefaultCredentials = FetchCredentialsOption.Include;

            app.AddComponent<App>("app");
        }
    }
}
