using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Tajan.ApiGateway.Helpers;

public static class CustomAuthentication
{
    public static void AddCustomAuthentication(this IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes("aasdjfhaskjfdsldlkasdmasdsadasdasdasdasdasdasdasdaddfdgfgbnfnnhgfnfgngfvcvcvcvcvcvcccccccccccccccccccccccccccccccccddfdfnnnnnbgbgbgbgbgbnfglkadjsakdjansjkdasndkjasndkasjndkajsdasdkasmdakd");
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("Bearer", x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                // Make roles work out of the box
                RoleClaimType = ClaimTypes.Role,         // matches how we emitted roles
                NameClaimType = ClaimTypes.NameIdentifier // if you prefer NameIdentifier as "User.Identity.Name"
            };
        }); ;
    }
}
