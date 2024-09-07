//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
using System.Text;
using System.Web.Routing;
using Sitecore.Configuration;

namespace Foundation.Wealth.Pipelines.Initialize
{
    public class Authorize
    {
        protected void Register(RouteCollection routes)
        {
            var jwtIssuer = Settings.GetSetting("JwtIssuer");
            var jwtAudience = Settings.GetSetting("JwtAudience");
            var jwtSecretKey = Settings.GetSetting("JwtSecretKey");

            //var jwtOptions = new JwtBearerAuthenticationOptions
            //{
            //    AuthenticationMode = AuthenticationMode.Active,
            //    TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = jwtIssuer,
            //        ValidAudience = jwtAudience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
            //    }
            //};
        }

    }
}
