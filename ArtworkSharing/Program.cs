using System;
using System.Reflection;
using System.Text.Json.Serialization;
using ArtworkSharing.Controllers;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Data;
using ArtworkSharing.Exceptions;
using ArtworkSharing.Extensions;
using ArtworkSharing.Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var ArtworkSharing = "ArtworkSharing";

// Add services to the container.
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("*")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
            //    .AllowCredentials(); 
        });
});

builder.Services.AddSession();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
 
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.ResolveConflictingActions(apiDescriptions =>
        apiDescriptions.OrderBy(action => action.RelativePath).First());
});

builder.Services.AddDbContext<ArtworkSharingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabase();
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.Expiration = TimeSpan.FromDays(7);
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.IsEssential = true;

    options.Cookie.SameSite = SameSiteMode.None;
    options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
    options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
    options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
    options.SlidingExpiration = true;
});
builder.Services.AddServices();
builder.Services.AddConfigException();
builder.Services.AddMvc(options => { options.SuppressAsyncSuffixInActionNames = false; });
builder.Services.AddHttpClient();

// Configure CORS to allow any origin
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});


// Đăng ký WatermarkController
builder.Services.AddTransient<WatermarkController>();
builder.Services.AddTransient<IWatermarkService, WatermarkService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
//app.UseCors();
app.UseCors("AllowOrigin");
app.UseSession();

EnsureMigrate(app);

//app.UseCors("AllowOrigin");
// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





//app.UseException();
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); // Add this line to enable authorization
app.UseMiddleware<ExceptionMiddleware>(); 

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();

void EnsureMigrate(WebApplication webApp)
{

   using var scope = webApp.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ArtworkSharingContext>();
   context.Database.Migrate();
}