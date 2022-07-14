using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

var builder = WebApplication.CreateBuilder(args);

var PetsDbConnectionString = builder.Configuration.GetConnectionString("PetsDb");
builder.Services.AddDbContext<PetsDBContext>(opt =>
{
    opt.UseSqlServer(PetsDbConnectionString);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
//登入Cookie設定
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Members/Login";
        opt.AccessDeniedPath = "/Members/AccessDenied";
    });
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

//驗證
app.UseAuthentication();
//授權
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
