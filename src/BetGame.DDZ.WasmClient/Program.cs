using BetGame.DDZ.WasmClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace BetGame.DDZ.WasmClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<FunctionHelper>();
            builder.Services.AddScoped<LocalStorage>();
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthStateProvider>());
            //在wasm中没有默认配置，所以需要设置一下
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            //builder.Services.AddAuthorizationCore(c=> {
            //    c.AddPolicy("default", a => a.RequireAuthenticatedUser());
            //    c.DefaultPolicy = c.GetPolicy("default");
            //});
            builder.Services.AddScoped(sp=>new ClientWebSocket());

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
