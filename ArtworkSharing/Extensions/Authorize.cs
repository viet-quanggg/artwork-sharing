using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ArtworkSharing.Extensions
{
    public class Authorize : Attribute, IAuthorizationFilter
    {
        private readonly IConfiguration _config;

        public Authorize()
        {
            _config = GetConfiguration();
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            StringValues authorizationHeader;
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationHeader))
            {
                return;
            }
            string tk = authorizationHeader + "";
            if (tk.Split(" ")[1] + "" != "")
            {
                await TokenHandle(tk.Split(" ")[1] + "", context.HttpContext);
            }

            throw new NotImplementedException();
        }
        public async Task TokenHandle(string token, HttpContext context)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!);
                tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken tokenHandled);
                var tokenJwt = (JwtSecurityToken)tokenHandled;
                var email = tokenJwt.Claims.First(x => x.Type == "email").Value;
                Guid id = Guid.Parse(tokenJwt.Claims.First(x => x.Type == "nameid").Value);
                context.Items["UserId"] = id;
                context.Items["Email"] = email;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            await Task.CompletedTask;
        }

        private IConfiguration GetConfiguration()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .BuildServiceProvider();

            return serviceProvider.GetService<IConfiguration>()!;
        }
    }
}
