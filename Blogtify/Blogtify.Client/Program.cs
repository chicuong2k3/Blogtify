using Blogtify.Client;
using Blogtify.Client.Theming;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlayVerse.Web.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IHttpContextProxy, WebAssemblyHttpContextProxy>();

builder.Services.AddCommonServices(builder.HostEnvironment, null);

//builder.Services.AddAuthorizationCore(options =>
//{

//});
//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
