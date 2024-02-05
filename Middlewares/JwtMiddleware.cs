
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace AllInOne.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {

        
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

           
            
            if (token != null)
            {
                var dedcodedToken = ValidateToken(context, token);
                if (dedcodedToken == null)
                {
                     Console.WriteLine(token);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Unauthorized" }));
                    return;
                }
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Unauthorized" }));
            }


    }

    private SecurityToken ValidateToken(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            IConfigurationSection JwtSettings = context.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(JwtSettings["SecretKey"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            
            return validatedToken;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

