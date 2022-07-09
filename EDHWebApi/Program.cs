using System.Net;
using System.Text.Json.Serialization;
using EDHWebApi.Persistance;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EDHContext>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("IsAdmin", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "Admin"));
    options.AddPolicy("IsUser", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "User"));
    options.AddPolicy("IsVerified", a=> a.RequireAuthenticatedUser().RequireClaim("isVerified", "isVerified"));

});


// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     
//     {
//         
//         options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//         options.SlidingExpiration = true;
//         options.AccessDeniedPath = "/Forbidden/";
//     });
builder.Services.AddAuthentication(options => { 
    options.DefaultScheme = "Cookies"; 
}).AddCookie("Cookies", options => {
    options.Cookie.Name = "Cookie_Name";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events = new CookieAuthenticationEvents
    {                          
        OnRedirectToLogin = redirectContext =>
        {
            redirectContext.HttpContext.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    };                
});

builder.Services.AddCors(options =>  
    options.AddPolicy("Development", builder =>  
    {  
        // Allow multiple HTTP methods  
        builder.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")  
            // .WithHeaders(  
            //     HeaderNames.Accept,  
            //     HeaderNames.ContentType,  
            //     HeaderNames.Authorization)  
            .AllowCredentials()  
            .SetIsOriginAllowed(origin =>  
            {  
                if (string.IsNullOrWhiteSpace(origin)) return false;  
                if (origin.ToLower().StartsWith("http://localhost:7213")) return true;  
                return false;  
            });  
    })  
);  




builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


var app = builder.Build();

var cookiePolicyOptions = new CookiePolicyOptions
{
    // MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.UseCors("Development");

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
//     endpoints.MapRazorPages();
// });

app.MapDefaultControllerRoute();
app.UseCookiePolicy(cookiePolicyOptions);


app.Run();

