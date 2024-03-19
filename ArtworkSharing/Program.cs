using System.Reflection;
using ArtworkSharing.Controllers;
using ArtworkSharing.DAL.Data;
using ArtworkSharing.Exceptions;
using ArtworkSharing.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddIdentityServices(builder.Configuration);


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
// Đăng ký WatermarkController
builder.Services.AddTransient<WatermarkController>();
var app = builder.Build();
EnsureMigrate(app);


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(_ => _.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseException();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ArtworkSharingContext>();
    context.Database.Migrate();
}