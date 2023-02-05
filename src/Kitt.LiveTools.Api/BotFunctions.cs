using Kitt.LiveTools.Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Kitt.LiveTools.Api;

public class BotFunctions
{
    private readonly ILogger _logger;

    public BotServices Services { get; }

    public BotFunctions(ILoggerFactory loggerFactory, BotServices services)
    {
        _logger = loggerFactory.CreateLogger<BotFunctions>();
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    [Function(nameof(StartBot))]
    public async Task<HttpResponseData> StartBot(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bot/start")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await Services.StartBotAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }

    [Function(nameof(StopBot))]
    public async Task<HttpResponseData> StopBot(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bot/stop")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await Services.StopBotAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}
