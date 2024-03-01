using Microsoft.AspNetCore.Authentication.JwtBearer;
using ArtworkSharing.DAL.Data;
using ArtworkSharing.Exceptions;
using ArtworkSharing.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ArtworkSharingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabase();
builder.Services.AddServices();
builder.Services.AddConfigException();
var app = builder.Build();
EnsureMigrate(app);
// Configure the HTTP request pipeline.
//app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseException();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ArtworkSharingContext>();
    context.Database.Migrate();
}
