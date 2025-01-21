/*
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
*/
/*app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
*/
using DemoApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DB context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StudentPortal")));


// Configure authentication with cookies
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // Path to the login page
        options.LogoutPath = "/User/Login"; // Path to the logout page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set session duration
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(); // Add authorization services

var app = builder.Build();

// Configure other middlewares (HTTPS, static files, etc.)
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Make sure authentication middleware comes before authorization
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
