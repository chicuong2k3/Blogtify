using Blogtify.Auth;
using Blogtify.Client;
using Blogtify.Client.Theming;
using Blogtify.Components;
using Blogtify.Configs;
using Blogtify.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCommonServices(null, builder.Configuration);

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IHttpContextProxy, ServerHttpContextProxy>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//})
//    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
//    {
//        var googleOptions = builder.Configuration.GetSection("Authentication:Google").Get<GoogleAuthOptions>()
//                                ?? throw new ArgumentNullException("Authentication:Google is missing.");
//        options.SignInScheme = IdentityConstants.ExternalScheme;

//        options.ClientId = googleOptions.ClientId;
//        options.ClientSecret = googleOptions.ClientSecret;
//    })
//    .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
//    {
//        var facebookOptions = builder.Configuration.GetSection("Authentication:Facebook").Get<FacebookAuthOptions>()
//                                    ?? throw new ArgumentNullException("Authentication:Facebook is missing.");
//        options.SignInScheme = IdentityConstants.ExternalScheme;
//        options.AppId = facebookOptions.AppId;
//        options.AppSecret = facebookOptions.AppSecret;
//        options.Scope.Add("email");
//        options.Scope.Add("public_profile");
//        options.ClaimActions.MapJsonKey("picture", "picture");
//    });


//builder.Services.AddAuthorization();

//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();
//builder.Services.AddHttpContextAccessor();

//builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseCors();

// Enable the .dat file extension (required to serve icudt.dat from _frameworkCompat/
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".dat"] = "application/octet-stream";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Blogtify.Client._Imports).Assembly);

app.MapControllers();
app.Run();
