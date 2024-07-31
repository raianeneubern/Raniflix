using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Raniflix.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// serviço de conexão com o Banco de Dados
string conn = builder.Configuration.GetConnectionString("RaniFlixConnection");
var version = ServerVersion.AutoDetect(conn);
builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseMySql(conn, version)
);

// Serviço de Gestão de Usuários
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    opt => opt.SignIn.RequireConfirmedAccount = false
)
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
