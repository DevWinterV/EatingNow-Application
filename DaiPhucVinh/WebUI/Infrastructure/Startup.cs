using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using DaiPhucVinh.Services.Constants;
using System.Text;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;


namespace DaiPhucVinh.Infrastructure
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable CORS for the SignalR hub
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJSONP = true; // Only use this for older browser support
            app.MapSignalR(hubConfiguration);
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtConfig.Issuer, //some string, normally web url,  
                        ValidAudience = JwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Secret))
                    }
                });
        }
    }
}