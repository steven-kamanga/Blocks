using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Components;
using WebApp.Services;
using WebApp.Services.Api;
using WebApp.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Blazor auth state
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<TokenStore>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

// Auth service (no auth header needed for login/register)
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5065/");
});

// Domain API services
var apiBase = new Uri("http://localhost:5065/");
builder.Services.AddHttpClient<ISiteService, SiteService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<IBatchService, BatchService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<IMaterialService, MaterialService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<IMachineService, MachineService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<IUserService, UserService>(c => c.BaseAddress = apiBase);

// ViewModels
builder.Services.AddScoped<HomeViewModel>();
builder.Services.AddScoped<SitesViewModel>();
builder.Services.AddScoped<BatchCalculatorViewModel>();
builder.Services.AddScoped<MaterialsViewModel>();
builder.Services.AddScoped<MachinesViewModel>();
builder.Services.AddScoped<MainLayoutViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Simple auth middleware: redirect to /login if no auth cookie
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";
    
    // Allow public paths through without auth check
    bool isPublicPath = path == "/login" || path == "/register" ||
                        path.StartsWith("/_") || path.Contains(".");
    
    if (!isPublicPath && !context.Request.Cookies.ContainsKey("auth_session"))
    {
        var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
        context.Response.Redirect($"/login?returnUrl={returnUrl}");
        return;
    }
    
    await next();
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
