using System.Text;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Client.Data.Validation;
using EDHWebApp.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EDHWebApp.Data;
using Hanssens.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ICompaniesData, CompanyDataService>();
builder.Services.AddSingleton<IUsersData, IUserDataService>();
builder.Services.AddSingleton<IUserLogInService, CloudUserLogInService>();
builder.Services.AddSingleton<IEmailSender, EmailSenderImpl>();
builder.Services.AddControllers();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
// builder.Services.AddSingleton<ILocalStorageService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.LoginPath="/";
    });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("IsAdmin", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "Admin"));
    options.AddPolicy("IsUser", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "User"));
    options.AddPolicy("IsVerified", a=> a.RequireAuthenticatedUser().RequireClaim("isVerified", "isVerified"));

});




var app = builder.Build();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
//     endpoints.MapRazorPages();
// });

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.UseCookiePolicy(cookiePolicyOptions);


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();