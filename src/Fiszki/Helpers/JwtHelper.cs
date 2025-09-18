using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fiszki.Models;
using Microsoft.IdentityModel.Tokens;

namespace Fiszki.Helpers;

internal static class JwtHelper
{
    public static string CreateJwtForUser(IConfiguration config, ApplicationUser user)
    {
        var issuer = config["Jwt:Issuer"] ?? "fiszki.local";
        var audience = config["Jwt:Audience"] ?? "fiszki.client";
        var key = config["Jwt:Key"] ?? "dev-secret-key-change-me";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? string.Empty)
        };
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
