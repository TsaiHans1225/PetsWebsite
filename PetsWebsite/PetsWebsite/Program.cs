using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;

var builder = WebApplication.CreateBuilder(args);

var PetsDbConnectionString = builder.Configuration.GetConnectionString("PetsDb");
builder.Services.AddDbContext<PetsDBContext>(opt =>
{
    opt.UseSqlServer(PetsDbConnectionString);
});

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddControllersWithViews();
//登入Cookie設定
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Members/Login";
        opt.AccessDeniedPath = "/Members/AccessDenied";
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//自訂Config串聯連線字串
DbConfig dbConfig = new DbConfig()
{
    ConnectionString = PetsDbConnectionString
};
builder.Services.AddSingleton(dbConfig.GetType(), dbConfig);

//自訂封裝連線資訊
IConfigurationSection section = builder.Configuration.GetSection("RestaurantService");
RestaurantService restaurantService = new RestaurantService();
section.Bind(restaurantService);
builder.Services.AddSingleton(restaurantService.GetType(), restaurantService);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
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
