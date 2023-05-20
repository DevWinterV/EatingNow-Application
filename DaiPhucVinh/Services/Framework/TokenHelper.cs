using Microsoft.IdentityModel.Tokens;
using DaiPhucVinh.Services.Constants;
using DaiPhucVinh.Shared.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace DaiPhucVinh.Services.Framework
{
    public class TokenHelper
    {
        public static string CreateToken(int expiresIn, Ticket ticket)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            claims.Add(new Claim("UserId", ticket.UserId.ToString()));
            claims.Add(new Claim("UserName", ticket.UserName));
            claims.Add(new Claim("Role", ticket.Role));
            if (ticket.Role != "Customer")
            {
                claims.Add(new Claim("RoleSystem", ticket.RoleSystem));

            }
            claims.Add(new Claim("LocationCode", ticket.LocationCode));

            var expiredsIn = expiresIn;//default 24 hours
            if (expiredsIn < 1)
            {
                expiredsIn = 24;
            }
            var token = new JwtSecurityToken(JwtConfig.Issuer, JwtConfig.Audience, claims, expires: DateTime.Now.Add(TimeSpan.FromHours(expiredsIn)), signingCredentials: credentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
        public static int GetCurrentUserId()
        {
            var identityModel = Thread.CurrentPrincipal;
            //var identityModel = HttpContext.Current.User;
            int userId = 0;
            if (identityModel.Identity.IsAuthenticated)
            {
                var identity = identityModel.Identity as ClaimsIdentity;
                if (identity != null && identity.Claims.Any())
                {
                    var value = identity.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
                    int.TryParse(value, out userId);
                }
            }
            return userId;
        }
        public static Ticket CurrentIdentity()
        {
            var identityModel = Thread.CurrentPrincipal;
            //var identityModel = HttpContext.Current.User;
            Ticket ticket = new Ticket();
            if (identityModel.Identity.IsAuthenticated)
            {
                var identity = identityModel.Identity as ClaimsIdentity;
                if (identity != null && identity.Claims.Any())
                {
                    ticket.UserId = int.Parse(identity.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);
                    ticket.Role = identity.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;
                    ticket.RoleSystem = identity.Claims.FirstOrDefault(x => x.Type == "RoleSystem")?.Value;
                    ticket.UserName = identity.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
                    ticket.LocationCode = identity.Claims.FirstOrDefault(x => x.Type == "LocationCode")?.Value;
                }
            }
            return ticket;
        }
    }
}
