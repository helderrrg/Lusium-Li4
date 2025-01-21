using Lusium.Components;
using Services;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database configuration.
builder.Services.AddDbContext<LusiumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services.
builder.Services.AddScoped<LusiumService>();

// Register HttpContextAccessor and custom service
builder.Services.AddControllers();
builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7240/");
});

//Login Service
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/";
        options.Cookie.SameSite = SameSiteMode.None; // Permite cookies entre origens
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Garante que o cookie seja enviado apenas em HTTPS
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Setup razor components.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Execute the application.
app.Run();
