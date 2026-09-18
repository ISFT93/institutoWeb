using instituto93.Web.Components;
using instituto93.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:8000";

if (!Uri.TryCreate(apiBaseUrl, UriKind.Absolute, out var apiUri))
    throw new InvalidOperationException("La variable de entorno Api__BaseUrl debe contener una URL absoluta.");

builder.Services.AddHttpClient<AuthApiClient>(client =>
{
    client.BaseAddress = new Uri($"{apiUri.AbsoluteUri.TrimEnd('/')}/");
});

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
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
