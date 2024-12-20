using Data; // Referência para o LusiumDbContext
using Microsoft.EntityFrameworkCore;
using Lusium.Components;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuração do banco de dados SQL Server usando Entity Framework Core
builder.Services.AddDbContext<LusiumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o LusiumService
builder.Services.AddScoped<LusiumService>();

// Registrar outros serviços necessários (por exemplo, para injeção de dependência)
builder.Services.AddScoped<LusiumService>(); // Caso tenha outros serviços a serem injetados

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    // Configurações específicas para produção
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); // Certifique-se de que o UseRouting está presente

// Configurar os componentes do Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Executar a aplicação
app.Run();


