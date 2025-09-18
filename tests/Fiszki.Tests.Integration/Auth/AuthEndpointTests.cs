using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class AuthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(_ => { });
    }

        [Fact]
    public async Task GET_Me_Returns401_WithoutToken()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/users/me");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GET_Me_Returns200_WithToken()
    {
        var client = _factory.CreateClient();
        var email = $"user{Guid.NewGuid():N}@test.local";
        var content = new StringContent($"email={Uri.EscapeDataString(email)}", Encoding.UTF8, "application/x-www-form-urlencoded");
        var tokenResp = await client.PostAsync("/api/auth/token", content);
        tokenResp.EnsureSuccessStatusCode();
        var json = await tokenResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(token));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var me = await client.GetAsync("/api/users/me");
        Assert.Equal(HttpStatusCode.OK, me.StatusCode);
    }
}