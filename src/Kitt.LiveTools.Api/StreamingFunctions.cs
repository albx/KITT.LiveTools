using Kitt.LiveTools.Api.Services;
using Kitt.LiveTools.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Kitt.LiveTools.Api;

public class StreamingFunctions
{
    private readonly ILogger _logger;

    public StreamingServices Services { get; }

    public StreamingFunctions(ILoggerFactory loggerFactory, StreamingServices services)
    {
        _logger = loggerFactory.CreateLogger<StreamingFunctions>();
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    [Function(nameof(GetStreamings))]
    public async Task<HttpResponseData> GetStreamings(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "streamings")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var streamings = await Services.GetScheduledStreamingsAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(streamings);

        return response;
    }

    [Function(nameof(SaveStreamingStats))]
    public async Task<HttpResponseData> SaveStreamingStats(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "streamings/stats")] HttpRequestData req)
    {
        var streamingStats = await req.ReadFromJsonAsync<StreamingStats>();
        if (streamingStats is null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        await Services.SaveStreamingStatsAsync(streamingStats);

        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}
