using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;
using TopLearn.Core.Services;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<FormOptions>(options =>
{
    // Set the limit to 256 MB
    options.MultipartBodyLengthLimit = 268435456;
    options.ValueLengthLimit = 268435456;
    options.MultipartHeadersLengthLimit = 268435456;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 268435456;
});

#region Db context

builder.Services.AddDbContext<TopLearnContext>(option =>
{
    option.UseSqlServer("Data Source=.; Initial Catalog=TopLearn_DB;Integrated Security=true;TrustServerCertificate=true;");
});

#endregion

#region IoC

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IOrderService, OrderService>();

#endregion

#region Authentication

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "/Login";
    option.LogoutPath = "/Logout";
    option.ExpireTimeSpan = TimeSpan.FromDays(7);
});

#endregion

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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
