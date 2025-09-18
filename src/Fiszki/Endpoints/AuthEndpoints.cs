using System.Security.Claims;
using Fiszki.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

namespace Fiszki.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var app = endpoints;

        app.MapGet("/api/auth/google", () => Results.Challenge(new AuthenticationProperties
        {
            RedirectUri = "/api/auth/google-callback"
        },
            [GoogleDefaults.AuthenticationScheme]))
            .ExcludeFromDescription();


        app.MapGet("/api/auth/google-callback", async (HttpContext http, UserManager<ApplicationUser> userManager, IConfiguration config) =>
        {
            var result = await http.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var principal = result?.Principal ?? http.User;
            var email = principal?.FindFirstValue(ClaimTypes.Email);
            var nameId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (email is null && nameId is null)
            {
                return Results.BadRequest(new { error = "Failed to authenticate with Google" });
            }

            var fallbackEmail = nameId != null ? nameId + "@google.local" : null;
            var user = await userManager.FindByEmailAsync(email ?? fallbackEmail);
            if (user is null)
            {
                user = new ApplicationUser { UserName = email ?? nameId!, Email = email };
                await userManager.CreateAsync(user);
            }

            var token = Helpers.JwtHelper.CreateJwtForUser(config, user);
            return Results.Ok(new { token });
        }).ExcludeFromDescription();

        // Development helper: issue a JWT for a given email (no Google roundtrip)
        app.MapPost("/api/auth/token", async (HttpRequest req, UserManager<ApplicationUser> userManager, IConfiguration config) =>
        {
            string? email = req.Query["email"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(email) && req.HasFormContentType)
            {
                var form = await req.ReadFormAsync();
                email = form["email"].FirstOrDefault();
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                return Results.BadRequest(new { error = "email is required" });
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new ApplicationUser { UserName = email, Email = email };
                await userManager.CreateAsync(user);
            }
            var token = Helpers.JwtHelper.CreateJwtForUser(config, user);
            return Results.Ok(new { token });
        }).WithTags("Auth").WithDescription("Development-only token issuer").DisableAntiforgery();

        return endpoints;
    }
}
