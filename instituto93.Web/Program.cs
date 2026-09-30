using instituto93.Web.Auth;
using instituto93.Web.Components;
using instituto93.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:8000";

if (!Uri.TryCreate(apiBaseUrl, UriKind.Absolute, out var apiUri))
    throw new InvalidOperationException("La variable de entorno Api__BaseUrl debe contener una URL absoluta.");

void ConfigureApiClient(HttpClient client) => client.BaseAddress = new Uri($"{apiUri.AbsoluteUri.TrimEnd('/')}/");

builder.Services.AddHttpClient<AuthApiClient>(ConfigureApiClient);
builder.Services.AddHttpClient<AccountApiClient>(ConfigureApiClient);

// BFF: el navegador solo recibe una cookie HttpOnly con el id de sesión; el access token y el
// refresh token quedan del lado del servidor. En memoria se pierden al reiniciar; para varias
// instancias o para que sobrevivan reinicios, reemplazar por AddStackExchangeRedisCache/AddDistributedSqlServerCache.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<TokenSessionStore>();
builder.Services.AddSingleton<LoginTicketStore>();
builder.Services.AddSingleton<SessionRefreshLocks>();
builder.Services.AddScoped<TokenSessionManager>();
builder.Services.AddScoped<AccessTokenProvider>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        var isDevelopment = builder.Environment.IsDevelopment();

        // El prefijo __Host- exige HTTPS; en desarrollo se sirve por http://localhost.
        options.Cookie.Name = isDevelopment ? "instituto93.auth" : "__Host-instituto93.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = isDevelopment ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
        options.LoginPath = LoginRedirect.LoginPath;
        options.AccessDeniedPath = LoginRedirect.LoginPath;
        options.SlidingExpiration = false;
        options.Events.OnValidatePrincipal = CookieSessionValidator.ValidateAsync;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, TokenRevalidatingAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapAccountEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
