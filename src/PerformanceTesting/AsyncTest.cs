using System.Diagnostics;
using Xunit.Abstractions;

namespace PerformanceTesting;

public class AsyncTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AsyncTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private const string BaseUrl = "http://localhost:5131";
    private const int VirtualUsers = 1000;
    private const int DurationSeconds = 30;

    /**
 * This methods handels request calls, and surves no other purpse than testing
 */
    private async Task ExecuteAndMeasureRequest(HttpClient httpClient, string method, List<long> responseTimes)
    {
        try
        {
            var requestUrl = $"{BaseUrl}/asynctesting/{method}";
            var stopwatch = Stopwatch.StartNew();
            HttpResponseMessage response;

            response = await httpClient.GetAsync(requestUrl);

            stopwatch.Stop();

            if (response.IsSuccessStatusCode)
            {
                responseTimes.Add(stopwatch.ElapsedMilliseconds);
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request exception: {ex.Message}");
        }
    }

    private static void LogPerformance(string testType, TimeSpan duration, int totalRequests, int successfulRequests,
        int errorCount,
        double errorRate, double averageResponseTimeMs, double requestsPerSecond)
    {
        Console.WriteLine($"Results for {testType} \n " +
                          $"======================================\n" +
                          "--- Performance Measurement ---\n" +
                          $"Test Duration: {duration.TotalSeconds:F2} seconds \n" +
                          $"Virtual Users: {VirtualUsers}\n" +
                          $"Total Requests: {totalRequests}\n" +
                          $"Successful Requests: {successfulRequests}\n" +
                          $"Error Count: {errorCount}\n" +
                          $"Error Rate: {errorRate:F2}%\n" +
                          $"Average Response Time: {averageResponseTimeMs:F2} ms\n" +
                          $"Requests Per Second (Throughput): {requestsPerSecond:F2}\n " +
                          $"======================================");
    }

    [Fact]
    public async Task AsyncTestSleep()
    {
        using (var httpClient = new HttpClient())
        {
            var tasks = new List<Task>();
            var responseTimes = new List<long>();
            var errorCount = 0;
            var startTime = DateTime.Now;

            for (int i = 0; i < VirtualUsers; i++)
            {
                tasks.Add(ExecuteAndMeasureRequest(httpClient, "sleep", responseTimes));
            }

            await Task.WhenAll(tasks);

            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            double averageResponseTimeMs = responseTimes.Any() ? responseTimes.Average() : 0;
            int successfulRequests = responseTimes.Count;
            int totalRequests = VirtualUsers; // In this simplified scenario
            errorCount = totalRequests - successfulRequests;
            double errorRate = totalRequests > 0 ? (double)errorCount / totalRequests * 100 : 0;
            double requestsPerSecond =
                duration.TotalSeconds > 0 ? successfulRequests / duration.TotalSeconds : 0;

            LogPerformance("AsyncTestSleep", duration, totalRequests, successfulRequests, errorCount, errorRate,
                averageResponseTimeMs, requestsPerSecond);
        }
    }

    [Fact]
    public async Task AsyncTestCalculate()
    {
        using (var httpClient = new HttpClient())
        {
            var tasks = new List<Task>();
            var responseTimes = new List<long>();
            var errorCount = 0;
            var startTime = DateTime.Now;

            for (int i = 0; i < VirtualUsers; i++)
            {
                tasks.Add(ExecuteAndMeasureRequest(httpClient, "calculate", responseTimes));
            }

            await Task.WhenAll(tasks);

            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            double averageResponseTimeMs = responseTimes.Any() ? responseTimes.Average() : 0;
            int successfulRequests = responseTimes.Count;
            int totalRequests = VirtualUsers; // In this simplified scenario
            errorCount = totalRequests - successfulRequests;
            double errorRate = totalRequests > 0 ? (double)errorCount / totalRequests * 100 : 0;
            double requestsPerSecond =
                duration.TotalSeconds > 0 ? successfulRequests / duration.TotalSeconds : 0;

            LogPerformance("AsyncTestCalculate", duration, totalRequests, successfulRequests, errorCount, errorRate,
                averageResponseTimeMs, requestsPerSecond);
        }
    }
}