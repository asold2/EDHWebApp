using System.Net;
using System.Text.Json.Serialization;
using EDHWebApi.Persistance;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EDHContext>();




// builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DbContext>(options =>
//     options.UseNpgsql(Configuration.GetConnectionStrig("DbContext")));

/*builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
    options.ListenAnyIP(5001, configure=> configure.UseHttps());
});*/

builder.WebHost.UseKestrel().UseContentRoot(Directory.GetCurrentDirectory()).UseIISIntegration();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("IsAdmin", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "Admin"));
    options.AddPolicy("IsUser", a=> a.RequireAuthenticatedUser().RequireClaim("Role", "User"));
    options.AddPolicy("IsVerified", a=> a.RequireAuthenticatedUser().RequireClaim("isVerified", "isVerified"));

});



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




using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<EDHContext>();
    dataContext.Database.Migrate();
}
//
var cookiePolicyOptions = new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
};
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});



// Configure the HTTP request pipeline.


app.UseHttpsRedirection();



app.MapControllers();




app.MapDefaultControllerRoute();
app.UseCookiePolicy(cookiePolicyOptions);


app.Run();

