using Blazored.LocalStorage;
using Blogtify.Client.Services;
using Blogtify.Client.Theming;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Blogtify.Client;

public static class ServicesConfigure
{
    public static void AddCommonServices(
        this IServiceCollection services,
        IWebAssemblyHostEnvironment? wasmEnv = null,
        IConfiguration? config = null)
    {
        services.AddHxServices();
        services.AddScoped<IThemeProvider, ThemeProvider>();
        services.AddBlazoredLocalStorage();
        services.AddHotKeys2();

        if (wasmEnv != null)
        {
            if (string.IsNullOrEmpty(wasmEnv.BaseAddress))
                throw new InvalidOperationException("WASM BaseAddress is missing. Cannot configure HttpClient.");

            services.AddSingleton<AppDataManager>(sp =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(wasmEnv.BaseAddress)
                };
                return new AppDataManager(client);
            });
        }
        else
        {
            var apiBase = config?["ApiBaseUrl"] ?? throw new ArgumentNullException("ApiBaseUrl is missing.");

            services.AddSingleton<AppDataManager>(sp =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(apiBase)
                };
                return new AppDataManager(client);
            });
        }



    }
}
