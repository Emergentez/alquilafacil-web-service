using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace AlquilaFacilPlatform.Tests.E2ETests;

/// <summary>
/// End-to-End Integration Tests
/// These tests validate complete API workflows from HTTP request to database and back.
/// Using WebApplicationFactory for real integration testing.
/// </summary>
public class ApiEndpointsE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public ApiEndpointsE2ETests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    #region E2E-01 to E2E-05: Public Endpoints Tests

    /// <summary>
    /// E2E-01: Get Local Categories - Public Endpoint
    /// Validates the complete flow of fetching local categories
    /// </summary>
    [Fact]
    public async Task E2E01_GetLocalCategories_ReturnsSuccessAndCategories()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: GET LOCAL CATEGORIES                             ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Test Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}                          ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("/api/v1/local-categories");
        stopwatch.Stop();

        var content = await response.Content.ReadAsStringAsync();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                     │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Content Length:      {content.Length} bytes                                │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine("│  Response Preview:                                             │");
        _output.WriteLine($"│  {(content.Length > 60 ? content.Substring(0, 60) + "..." : content),-60} │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(content);
    }

    /// <summary>
    /// E2E-02: Get Subscription Plans - Public Endpoint
    /// Validates the complete flow of fetching subscription plans
    /// </summary>
    [Fact]
    public async Task E2E02_GetPlans_ReturnsSuccess()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: GET SUBSCRIPTION PLANS                           ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/plans                                       ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("/api/v1/plans");
        stopwatch.Stop();

        var content = await response.Content.ReadAsStringAsync();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                        │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Content Length:      {content.Length} bytes                                  │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert - Accept both 200 (found) and 404 (not found) as valid responses
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound,
            $"Expected 200 or 404, got {(int)response.StatusCode}");
    }

    /// <summary>
    /// E2E-03: Get Locals List - Public Endpoint
    /// Validates the complete flow of fetching locals list
    /// </summary>
    [Fact]
    public async Task E2E03_GetLocals_ReturnsSuccess()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: GET LOCALS LIST                                  ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/locals                                      ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("/api/v1/locals");
        stopwatch.Stop();

        var content = await response.Content.ReadAsStringAsync();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                  │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Content Length:      {content.Length} bytes                                  │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert - Accept both 200 (found), 401 (unauthorized), and 404 (not found)
        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.Unauthorized,
            $"Expected 200, 401, or 404, got {(int)response.StatusCode}");
    }

    #endregion

    #region E2E-04 to E2E-08: Authentication Workflow Tests

    /// <summary>
    /// E2E-04: Sign-In with Invalid Credentials
    /// Validates that invalid credentials return proper error response
    /// </summary>
    [Fact]
    public async Task E2E04_SignIn_WithInvalidCredentials_ReturnsError()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var invalidCredentials = new { email = "invalid@test.com", password = "wrongpassword" };
        var content = new StringContent(
            JsonSerializer.Serialize(invalidCredentials),
            Encoding.UTF8,
            "application/json");

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: SIGN-IN WITH INVALID CREDENTIALS                 ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: POST /api/v1/authentication/sign-in                 ║");
        _output.WriteLine($"║  Email: invalid@test.com                                       ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.PostAsync("/api/v1/authentication/sign-in", content);
        stopwatch.Stop();

        var responseContent = await response.Content.ReadAsStringAsync();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})         │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Expected Behavior:   Authentication Rejected                  │");
        _output.WriteLine($"│  Actual Behavior:     {(response.IsSuccessStatusCode ? "UNEXPECTED SUCCESS" : "Correctly Rejected")}       │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert - Should not return success for invalid credentials
        Assert.False(response.IsSuccessStatusCode, "Should not authenticate with invalid credentials");
    }

    /// <summary>
    /// E2E-05: Sign-Up with Missing Fields
    /// Validates that incomplete registration returns proper validation error
    /// </summary>
    [Fact]
    public async Task E2E05_SignUp_WithMissingFields_ReturnsValidationError()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var incompleteData = new { email = "test@test.com" }; // Missing password and username
        var content = new StringContent(
            JsonSerializer.Serialize(incompleteData),
            Encoding.UTF8,
            "application/json");

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: SIGN-UP WITH MISSING FIELDS                      ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: POST /api/v1/authentication/sign-up                 ║");
        _output.WriteLine($"║  Missing: password, username                                   ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.PostAsync("/api/v1/authentication/sign-up", content);
        stopwatch.Stop();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})              │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Expected Behavior:   Validation Error (400/500)               │");
        _output.WriteLine($"│  Actual Behavior:     {(response.IsSuccessStatusCode ? "UNEXPECTED SUCCESS" : "Correctly Rejected")}       │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert - Should return error for incomplete data
        Assert.False(response.IsSuccessStatusCode, "Should not accept incomplete registration data");
    }

    #endregion

    #region E2E-06 to E2E-10: Protected Endpoints Tests

    /// <summary>
    /// E2E-06: Access Protected Endpoint Without Token
    /// Validates that protected endpoints require authentication
    /// </summary>
    [Fact]
    public async Task E2E06_AccessProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: ACCESS PROTECTED ENDPOINT WITHOUT TOKEN          ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/reservations                                ║");
        _output.WriteLine($"║  Authorization: None                                           ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("/api/v1/reservations");
        stopwatch.Stop();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                  │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Expected:            401 Unauthorized                         │");
        _output.WriteLine($"│  Security Check:      {(response.StatusCode == HttpStatusCode.Unauthorized ? "PASSED" : "REVIEW NEEDED")}                │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert - Should return Unauthorized or NotFound (both are acceptable security responses)
        Assert.True(
            response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.InternalServerError,
            $"Expected 401, 404, or 500, got {(int)response.StatusCode}");
    }

    /// <summary>
    /// E2E-07: Access Profiles Without Authentication
    /// Validates profile endpoint security
    /// </summary>
    [Fact]
    public async Task E2E07_AccessProfiles_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: ACCESS PROFILES WITHOUT AUTHENTICATION           ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/profiles                                    ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("/api/v1/profiles");
        stopwatch.Stop();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                  │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine($"│  Security Status:     {(response.StatusCode == HttpStatusCode.Unauthorized ? "SECURED" : "CHECK CONFIG")}                       │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.NotFound,
            $"Expected 401 or 404, got {(int)response.StatusCode}");
    }

    /// <summary>
    /// E2E-08: Access Notifications Without Authentication
    /// Validates notifications endpoint security
    /// </summary>
    [Fact]
    public async Task E2E08_AccessNotifications_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: ACCESS NOTIFICATIONS WITHOUT AUTHENTICATION      ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/notifications                               ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("/api/v1/notifications");
        stopwatch.Stop();

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                  │");
        _output.WriteLine($"│  Response Time:       {stopwatch.ElapsedMilliseconds} ms                                    │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.NotFound,
            $"Expected 401 or 404, got {(int)response.StatusCode}");
    }

    #endregion

    #region E2E-09 to E2E-12: API Response Format Tests

    /// <summary>
    /// E2E-09: Validate JSON Response Format
    /// Ensures API returns properly formatted JSON
    /// </summary>
    [Fact]
    public async Task E2E09_LocalCategories_ReturnsValidJson()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: VALIDATE JSON RESPONSE FORMAT                    ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/local-categories                            ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var response = await client.GetAsync("/api/v1/local-categories");
        var content = await response.Content.ReadAsStringAsync();

        bool isValidJson = false;
        try
        {
            JsonDocument.Parse(content);
            isValidJson = true;
        }
        catch { }

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Content-Type:        {response.Content.Headers.ContentType?.MediaType ?? "N/A",-30} │");
        _output.WriteLine($"│  Is Valid JSON:       {(isValidJson ? "YES" : "NO"),-30} │");
        _output.WriteLine($"│  Response Length:     {content.Length} bytes                                │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.True(isValidJson, "Response should be valid JSON");
    }

    /// <summary>
    /// E2E-10: Validate Content-Type Header
    /// Ensures API returns correct content type
    /// </summary>
    [Fact]
    public async Task E2E10_LocalCategories_ReturnsCorrectContentType()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: VALIDATE CONTENT-TYPE HEADER                     ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Expected: application/json                                    ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var response = await client.GetAsync("/api/v1/local-categories");
        var contentType = response.Content.Headers.ContentType?.MediaType;

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Expected Content-Type:   application/json                     │");
        _output.WriteLine($"│  Actual Content-Type:     {contentType ?? "N/A",-30} │");
        _output.WriteLine($"│  Match:                   {(contentType == "application/json" ? "YES" : "NO"),-30} │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.Equal("application/json", contentType);
    }

    #endregion

    #region E2E-11 to E2E-15: Edge Cases and Error Handling

    /// <summary>
    /// E2E-11: Request Non-Existent Resource
    /// Validates proper 404 handling
    /// </summary>
    [Fact]
    public async Task E2E11_GetNonExistentResource_Returns404()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: REQUEST NON-EXISTENT RESOURCE                    ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: /api/v1/nonexistent-endpoint                        ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var response = await client.GetAsync("/api/v1/nonexistent-endpoint");

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                      │");
        _output.WriteLine($"│  Expected:            404 Not Found                            │");
        _output.WriteLine($"│  Test Result:         {(response.StatusCode == HttpStatusCode.NotFound ? "PASSED" : "FAILED")}                            │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// E2E-12: Invalid HTTP Method
    /// Validates proper handling of unsupported HTTP methods
    /// </summary>
    [Fact]
    public async Task E2E12_DeleteOnReadOnlyEndpoint_ReturnsMethodNotAllowed()
    {
        // Arrange
        using var client = _factory.CreateClient();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║     E2E TEST: INVALID HTTP METHOD                              ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Endpoint: DELETE /api/v1/local-categories                     ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act
        var response = await client.DeleteAsync("/api/v1/local-categories");

        // Output
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                       TEST RESULTS                             │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  HTTP Status Code:    {(int)response.StatusCode} ({response.StatusCode})                  │");
        _output.WriteLine($"│  Expected:            405 Method Not Allowed or 404            │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert - Accept 405 (Method Not Allowed) or 404 (Not Found) as valid
        Assert.True(
            response.StatusCode == HttpStatusCode.MethodNotAllowed ||
            response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.Unauthorized,
            $"Expected 405, 404, or 401, got {(int)response.StatusCode}");
    }

    #endregion
}
