﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Logic;
using PetsWebsite.Models;
using PetsWebsite.Models.Repository;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var PetsDbConnectionString = builder.Configuration.GetConnectionString("PetsDb");
builder.Services.AddDbContext<PetsDBContext>(opt =>
{
    opt.UseSqlServer(PetsDbConnectionString);
});
builder.Services.AddTransient<IcommonLogic, CommonLogic>();
builder.Services.AddTransient<RestaurantsRepository>();
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddControllersWithViews();
//登入Cookie設定
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Members/Login";
        opt.AccessDeniedPath = "/Members/AccessDenied";
    }).AddFacebook(opt =>
    {
        opt.AppId = "788179852360352";
        opt.AppSecret = "4aca3eb028c6422ff9df319c6746d592";
        opt.Events = new OAuthEvents
        {
            OnTicketReceived = ctx =>
            {
                var db = ctx.HttpContext.RequestServices.GetRequiredService<PetsDBContext>();
                var email = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var LastName = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                var FirstName = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var LoginProvider = ctx.Principal.Claims.Select(c => c.Issuer).First();
                var ProviderKey = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = db.UserLogins.FirstOrDefault(x => x.User.Email == email);
                if (user == null && string.IsNullOrEmpty(user?.ProviderKey))
                {
                    //create user info
                    UserLogin Member = new UserLogin()
                    {
                        LoginProvider = LoginProvider,
                        ProviderKey = ProviderKey,
                        User = new User()
                        {
                            LastName = LastName,
                            FirstName = FirstName,
                            Email = email,
                            RoleId = 1,
                        }
                    };
                    db.UserLogins.Add(Member);
                    db.SaveChanges();
                    var NewMember = db.UserLogins.FirstOrDefault(x => x.ProviderKey == ProviderKey);
                    user = NewMember;
                }
                else if (string.IsNullOrEmpty(user.ProviderKey))
                {
                    user.ProviderKey = ProviderKey;
                    user.LoginProvider = LoginProvider;
                    db.SaveChanges();
                }
                // add claims
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Member"),
                new Claim("UserID",user.UserId.ToString())
            };
                ctx.Principal.Identities.First().AddClaims(claims);
                return Task.CompletedTask;
            },
        };
    }).AddGoogle(opt =>
    {
        opt.ClientId = builder.Configuration["oAuth:GoogleClientID"];
        opt.ClientSecret = builder.Configuration["oAuth:GoogleClientSecret"];
        opt.Events = new OAuthEvents
        {
            OnTicketReceived = ctx =>
            {
                var db = ctx.HttpContext.RequestServices.GetRequiredService<PetsDBContext>();
                var email = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var LastName = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                var FirstName = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var LoginProvider = ctx.Principal.Claims.Select(c => c.Issuer).First();
                var ProviderKey = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = db.UserLogins.FirstOrDefault(x => x.User.Email == email);
                if (user == null && string.IsNullOrEmpty(user?.ProviderKey))
                {
                    //create user info
                    UserLogin Member = new UserLogin()
                    {
                        LoginProvider = LoginProvider,
                        ProviderKey = ProviderKey,
                        User = new User()
                        {
                            LastName = LastName,
                            FirstName = FirstName,
                            Email = email,
                            RoleId = 1,
                        }
                    };
                    db.UserLogins.Add(Member);
                    db.SaveChanges();
                    var NewMember = db.UserLogins.FirstOrDefault(x => x.ProviderKey == ProviderKey);
                    user = NewMember;
                }
                else if (string.IsNullOrEmpty(user.ProviderKey))
                {
                    user.ProviderKey = ProviderKey;
                    user.LoginProvider = LoginProvider;
                    db.SaveChanges();
                }
                // add claims
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Member"),
                new Claim("UserID",user.UserId.ToString())
            };
                ctx.Principal.Identities.First().AddClaims(claims);
                return Task.CompletedTask;
            },
        };
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

PetsAboptionResource petsRes = new PetsAboptionResource();
builder.Configuration.GetSection("PetsAboptionResource").Bind(petsRes);
builder.Services.AddSingleton(petsRes.GetType(), petsRes);

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
