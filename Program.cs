using Identity.Data;
using Identity.IdentityPolicy;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);


if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();    
}
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlite("Data Source=app.db"));

builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 4;
});
builder.Services.AddTransient<IPasswordValidator<AppUser>,CustomPasswordPolicy>();
builder.Services.AddTransient<IUserValidator<AppUser>,CustomUsernameEmailPolicy>();

builder.Services.ConfigureApplicationCookie(opts => 
{
    opts.LoginPath = "/Account/Login";
    opts.Cookie.Name = ".AspNetCore.Identity.Application";
    opts.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    opts.SlidingExpiration = true;
});

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AspManager", policy =>
    {
        policy.RequireRole("Manager");
        policy.RequireClaim("Coding-Skill", "ASP.NET Core MVC");
    });
});

// Add services to the container.
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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeed.SeedAsync(userManager,roleManager);
}

app.Run();
