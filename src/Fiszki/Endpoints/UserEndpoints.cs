using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Fiszki.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users/me", [Authorize] (ClaimsPrincipal user) =>
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var email = user.FindFirstValue(ClaimTypes.Email);
            var name = user.Identity?.Name ?? user.FindFirstValue(ClaimTypes.Name);
            return Results.Ok(new { sub, email, name });
        }).WithTags("Users");

        return endpoints;
    }
}
