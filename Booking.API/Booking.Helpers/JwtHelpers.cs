using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Booking.Helpers
{
    public class JwtHelpers
    {
        public static string IssueToken(string appName, string appUrl, string email, int authLifetimeInHours, string secret)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: appName,
                    audience: appUrl,
                    notBefore: now,
                    claims: new List<Claim> { new Claim("email", email) },
                    expires: now.Add(TimeSpan.FromHours(authLifetimeInHours)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                                            Encoding.ASCII.GetBytes(secret)),
                                            SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
