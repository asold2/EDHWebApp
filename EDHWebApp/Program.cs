using Client.Authentication;
using Client.Data.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EDHWebApp.Data;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ICompaniesData, CompanyDataService>();
builder.Services.AddSingleton<IUsersData, IUserDataService>();
builder.Services.AddSingleton<IUserLogInService, CloudUserLogInService>();
builder.Services.AddSingleton<IEmailSender, EmailSenderImpl>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();



builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("IsAdmin", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "Admin"));
    options.AddPolicy("IsUser", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "User"));
    options.AddPolicy("IsVerified", a=> a.RequireAuthenticatedUser().RequireClaim("isVerified", "isVerified"));

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();