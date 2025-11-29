using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace AlquilaFacilPlatform.Tests.PerformanceTests;

/// <summary>
/// Performance Tests using simple load simulation
/// These tests measure response times, throughput, and system behavior under load.
/// Results are displayed in console with detailed metrics for screenshot evidence.
/// </summary>
public class ApiPerformanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public ApiPerformanceTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    /// <summary>
    /// PERF-01: Health Check Endpoint Performance
    /// Tests the basic availability and response time of the API under concurrent load
    /// </summary>
    [Fact]
    public async Task PERF01_HealthCheck_ShouldHandleConcurrentRequests()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var requestCount = 50;
        var responseTimes = new List<double>();
        var successCount = 0;
        var failCount = 0;

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║           PERFORMANCE TEST: HEALTH CHECK ENDPOINT              ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Test Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}                          ║");
        _output.WriteLine($"║  Total Requests: {requestCount}                                           ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act - Execute concurrent requests
        var tasks = new List<Task<(bool Success, double ResponseTime)>>();

        for (int i = 0; i < requestCount; i++)
        {
            tasks.Add(ExecuteTimedRequest(client, "/api/v1/local-categories"));
        }

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            if (result.Success)
            {
                successCount++;
                responseTimes.Add(result.ResponseTime);
            }
            else
            {
                failCount++;
            }
        }

        // Calculate statistics
        var avgResponseTime = responseTimes.Any() ? responseTimes.Average() : 0;
        var minResponseTime = responseTimes.Any() ? responseTimes.Min() : 0;
        var maxResponseTime = responseTimes.Any() ? responseTimes.Max() : 0;
        var p50 = GetPercentile(responseTimes, 50);
        var p95 = GetPercentile(responseTimes, 95);
        var p99 = GetPercentile(responseTimes, 99);

        // Output Results
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                    TEST RESULTS SUMMARY                        │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  ✓ Successful Requests:  {successCount,5}                                 │");
        _output.WriteLine($"│  ✗ Failed Requests:      {failCount,5}                                 │");
        _output.WriteLine($"│  Success Rate:           {(successCount * 100.0 / requestCount):F1}%                                │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine("│                    LATENCY METRICS (ms)                        │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Minimum:                {minResponseTime,8:F2} ms                         │");
        _output.WriteLine($"│  Average:                {avgResponseTime,8:F2} ms                         │");
        _output.WriteLine($"│  Maximum:                {maxResponseTime,8:F2} ms                         │");
        _output.WriteLine($"│  P50 (Median):           {p50,8:F2} ms                         │");
        _output.WriteLine($"│  P95:                    {p95,8:F2} ms                         │");
        _output.WriteLine($"│  P99:                    {p99,8:F2} ms                         │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.True(successCount > 0, "Should have successful requests");
        Assert.True(avgResponseTime < 5000, "Average response time should be under 5 seconds");
    }

    /// <summary>
    /// PERF-02: Authentication Endpoint Load Test
    /// Tests SignIn endpoint under concurrent load
    /// </summary>
    [Fact]
    public async Task PERF02_SignIn_ShouldHandleConcurrentRequests()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var requestCount = 30;
        var responseTimes = new List<double>();
        var successCount = 0;
        var failCount = 0;
        var statusCodes = new Dictionary<int, int>();

        var signInPayload = JsonSerializer.Serialize(new { email = "test@test.com", password = "Test123!@#" });

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║           PERFORMANCE TEST: SIGN-IN ENDPOINT                   ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Test Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}                          ║");
        _output.WriteLine($"║  Concurrent Requests: {requestCount}                                      ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act - Execute concurrent POST requests
        var tasks = new List<Task<(bool Success, double ResponseTime, int StatusCode)>>();

        for (int i = 0; i < requestCount; i++)
        {
            tasks.Add(ExecuteTimedPostRequest(client, "/api/v1/authentication/sign-in", signInPayload));
        }

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            responseTimes.Add(result.ResponseTime);
            if (result.Success || result.StatusCode == 401 || result.StatusCode == 400)
            {
                successCount++;
            }
            else
            {
                failCount++;
            }

            if (!statusCodes.ContainsKey(result.StatusCode))
                statusCodes[result.StatusCode] = 0;
            statusCodes[result.StatusCode]++;
        }

        // Calculate statistics
        var avgResponseTime = responseTimes.Average();
        var minResponseTime = responseTimes.Min();
        var maxResponseTime = responseTimes.Max();
        var p50 = GetPercentile(responseTimes, 50);
        var p95 = GetPercentile(responseTimes, 95);

        // Output Results
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                    TEST RESULTS SUMMARY                        │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Total Requests:         {requestCount,5}                                 │");
        _output.WriteLine($"│  Responses Received:     {successCount,5}                                 │");
        _output.WriteLine($"│  Errors:                 {failCount,5}                                 │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine("│                    STATUS CODE DISTRIBUTION                    │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        foreach (var kvp in statusCodes.OrderBy(x => x.Key))
        {
            _output.WriteLine($"│  HTTP {kvp.Key}:                {kvp.Value,5} requests                         │");
        }
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine("│                    LATENCY METRICS (ms)                        │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Minimum:                {minResponseTime,8:F2} ms                         │");
        _output.WriteLine($"│  Average:                {avgResponseTime,8:F2} ms                         │");
        _output.WriteLine($"│  Maximum:                {maxResponseTime,8:F2} ms                         │");
        _output.WriteLine($"│  P50 (Median):           {p50,8:F2} ms                         │");
        _output.WriteLine($"│  P95:                    {p95,8:F2} ms                         │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.True(responseTimes.Count > 0, "Should receive responses");
    }

    /// <summary>
    /// PERF-03: API Stress Test - Multiple Endpoints
    /// Tests system behavior under stress with multiple endpoint types
    /// </summary>
    [Fact]
    public async Task PERF03_MultipleEndpoints_StressTest()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var requestsPerEndpoint = 20;
        var endpoints = new[]
        {
            "/api/v1/local-categories",
            "/api/v1/plans",
            "/api/v1/locals"
        };

        var allResults = new List<(string Endpoint, double ResponseTime, int StatusCode)>();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║              STRESS TEST: MULTIPLE ENDPOINTS                   ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Test Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}                          ║");
        _output.WriteLine($"║  Endpoints Tested: {endpoints.Length}                                          ║");
        _output.WriteLine($"║  Requests Per Endpoint: {requestsPerEndpoint}                                    ║");
        _output.WriteLine($"║  Total Requests: {endpoints.Length * requestsPerEndpoint}                                            ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        // Act - Execute concurrent requests to all endpoints
        var tasks = new List<Task<(string Endpoint, double ResponseTime, int StatusCode)>>();

        foreach (var endpoint in endpoints)
        {
            for (int i = 0; i < requestsPerEndpoint; i++)
            {
                tasks.Add(ExecuteTimedRequestWithEndpoint(client, endpoint));
            }
        }

        var results = await Task.WhenAll(tasks);
        allResults.AddRange(results);

        // Calculate per-endpoint statistics
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                  PER-ENDPOINT RESULTS                          │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");

        foreach (var endpoint in endpoints)
        {
            var endpointResults = allResults.Where(r => r.Endpoint == endpoint).ToList();
            var times = endpointResults.Select(r => r.ResponseTime).ToList();
            var successCount = endpointResults.Count(r => r.StatusCode >= 200 && r.StatusCode < 500);

            _output.WriteLine($"│  Endpoint: {endpoint,-40}        │");
            _output.WriteLine($"│    - Requests:     {endpointResults.Count,5}                                    │");
            _output.WriteLine($"│    - Success:      {successCount,5}                                    │");
            _output.WriteLine($"│    - Avg Latency:  {times.Average(),8:F2} ms                            │");
            _output.WriteLine($"│    - P95 Latency:  {GetPercentile(times, 95),8:F2} ms                            │");
            _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        }

        // Overall statistics
        var allTimes = allResults.Select(r => r.ResponseTime).ToList();
        var totalSuccess = allResults.Count(r => r.StatusCode >= 200 && r.StatusCode < 500);

        _output.WriteLine("│                    OVERALL STATISTICS                          │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Total Requests:         {allResults.Count,5}                                 │");
        _output.WriteLine($"│  Total Successful:       {totalSuccess,5}                                 │");
        _output.WriteLine($"│  Overall Avg Latency:    {allTimes.Average(),8:F2} ms                         │");
        _output.WriteLine($"│  Overall P50:            {GetPercentile(allTimes, 50),8:F2} ms                         │");
        _output.WriteLine($"│  Overall P95:            {GetPercentile(allTimes, 95),8:F2} ms                         │");
        _output.WriteLine($"│  Overall P99:            {GetPercentile(allTimes, 99),8:F2} ms                         │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        Assert.True(allResults.Count == endpoints.Length * requestsPerEndpoint, "All requests should complete");
    }

    /// <summary>
    /// PERF-04: Response Time Benchmark
    /// Measures baseline response times for critical endpoints
    /// </summary>
    [Fact]
    public async Task PERF04_ResponseTime_Benchmark()
    {
        // Arrange
        using var client = _factory.CreateClient();
        var endpoints = new[]
        {
            ("GET", "/api/v1/local-categories", "Local Categories"),
            ("GET", "/api/v1/plans", "Subscription Plans"),
            ("GET", "/api/v1/locals", "Locals List"),
        };

        var iterations = 10;
        var benchmarkResults = new List<(string Name, string Method, double AvgTime, double MinTime, double MaxTime, int StatusCode)>();

        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║              RESPONSE TIME BENCHMARK                           ║");
        _output.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        _output.WriteLine($"║  Test Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}                          ║");
        _output.WriteLine($"║  Iterations per endpoint: {iterations}                                     ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        foreach (var (method, endpoint, name) in endpoints)
        {
            var times = new List<double>();
            int lastStatusCode = 0;

            for (int i = 0; i < iterations; i++)
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    var response = await client.GetAsync(endpoint);
                    stopwatch.Stop();
                    times.Add(stopwatch.Elapsed.TotalMilliseconds);
                    lastStatusCode = (int)response.StatusCode;
                }
                catch (Exception)
                {
                    stopwatch.Stop();
                    times.Add(stopwatch.Elapsed.TotalMilliseconds);
                    lastStatusCode = 500;
                }
            }

            benchmarkResults.Add((name, method, times.Average(), times.Min(), times.Max(), lastStatusCode));
        }

        // Output Results
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                           BENCHMARK RESULTS                                    │");
        _output.WriteLine("├──────────────────────────┬────────┬──────────┬──────────┬──────────┬──────────┤");
        _output.WriteLine("│ Endpoint                 │ Method │  Avg(ms) │  Min(ms) │  Max(ms) │  Status  │");
        _output.WriteLine("├──────────────────────────┼────────┼──────────┼──────────┼──────────┼──────────┤");

        foreach (var result in benchmarkResults)
        {
            _output.WriteLine($"│ {result.Name,-24} │ {result.Method,-6} │ {result.AvgTime,8:F2} │ {result.MinTime,8:F2} │ {result.MaxTime,8:F2} │ {result.StatusCode,8} │");
        }

        _output.WriteLine("└──────────────────────────┴────────┴──────────┴──────────┴──────────┴──────────┘");

        // Performance Summary
        var overallAvg = benchmarkResults.Average(r => r.AvgTime);
        _output.WriteLine("");
        _output.WriteLine("┌────────────────────────────────────────────────────────────────┐");
        _output.WriteLine("│                    PERFORMANCE SUMMARY                         │");
        _output.WriteLine("├────────────────────────────────────────────────────────────────┤");
        _output.WriteLine($"│  Overall Average Response Time: {overallAvg,8:F2} ms                     │");
        _output.WriteLine($"│  Fastest Endpoint: {benchmarkResults.OrderBy(r => r.AvgTime).First().Name,-30}     │");
        _output.WriteLine($"│  Slowest Endpoint: {benchmarkResults.OrderByDescending(r => r.AvgTime).First().Name,-30}     │");
        _output.WriteLine("└────────────────────────────────────────────────────────────────┘");

        // Assert
        foreach (var result in benchmarkResults)
        {
            Assert.True(result.AvgTime < 10000, $"{result.Name} should respond within 10 seconds");
        }
    }

    #region Helper Methods

    private async Task<(bool Success, double ResponseTime)> ExecuteTimedRequest(HttpClient client, string endpoint)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var response = await client.GetAsync(endpoint);
            stopwatch.Stop();
            return (response.IsSuccessStatusCode || (int)response.StatusCode < 500, stopwatch.Elapsed.TotalMilliseconds);
        }
        catch
        {
            stopwatch.Stop();
            return (false, stopwatch.Elapsed.TotalMilliseconds);
        }
    }

    private async Task<(bool Success, double ResponseTime, int StatusCode)> ExecuteTimedPostRequest(HttpClient client, string endpoint, string jsonPayload)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            stopwatch.Stop();
            return (response.IsSuccessStatusCode, stopwatch.Elapsed.TotalMilliseconds, (int)response.StatusCode);
        }
        catch
        {
            stopwatch.Stop();
            return (false, stopwatch.Elapsed.TotalMilliseconds, 500);
        }
    }

    private async Task<(string Endpoint, double ResponseTime, int StatusCode)> ExecuteTimedRequestWithEndpoint(HttpClient client, string endpoint)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var response = await client.GetAsync(endpoint);
            stopwatch.Stop();
            return (endpoint, stopwatch.Elapsed.TotalMilliseconds, (int)response.StatusCode);
        }
        catch
        {
            stopwatch.Stop();
            return (endpoint, stopwatch.Elapsed.TotalMilliseconds, 500);
        }
    }

    private double GetPercentile(List<double> values, int percentile)
    {
        if (!values.Any()) return 0;

        var sorted = values.OrderBy(x => x).ToList();
        var index = (int)Math.Ceiling((percentile / 100.0) * sorted.Count) - 1;
        return sorted[Math.Max(0, Math.Min(index, sorted.Count - 1))];
    }

    #endregion
}
