using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SIGES.Config;
using SIGES.Data;
using SIGES.Middleware;
using SIGES.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var cultureInfo = new CultureInfo("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddDistributedMemoryCache(); // Necessário para armazenar sessões na memória
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define o tempo de expiração
    options.Cookie.HttpOnly = true; // Protege contra acesso via JavaScript
    options.Cookie.IsEssential = true; // Necessário para conformidade com LGPD/GDPR
});

// Adicionar o contexto do Entity Framework
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        new MySqlServerVersion(new Version(8, 0, 32)) // Substitua pela versão do MySQL que você está usando
    ));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Login/Login";
    options.LogoutPath = "/Login/Logout";
    options.AccessDeniedPath = "/Login/AcessoNegado";
    options.Cookie.Name = "UserLoginCookie"; // Nome do cookie
    options.ExpireTimeSpan = TimeSpan.FromHours(1); // Tempo padrão de expiração
    //options.SlidingExpiration = true; // Renova a expiração a cada requisição
});

builder.Services.AddAuthorization();

// Configurar repositórios de maneira centralizada
RepoConfig.RegistrarRepositorios(builder.Services);
builder.Services.AddSingleton<AppSettingsService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new AppSettingsService(configuration);
});
builder.Services.AddSingleton<PaymentGatewaysService>();
builder.Services.AddScoped<TemaService>();
builder.Services.AddSingleton<PaymentGatewaysService>();
builder.Services.AddScoped<CobrancaService>();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PermissaoMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "categorias",
    pattern: "categorias/{action}/{id?}",
    defaults: new { controller = "Categoria" });

app.MapControllerRoute(
    name: "usuarios",
    pattern: "usuarios/{action}/{id?}",
    defaults: new { controller = "Usuario" });

app.Run();
