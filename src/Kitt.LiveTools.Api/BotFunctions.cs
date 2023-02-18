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

        try
        {
            await Services.StartBotAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        catch (HttpRequestException ex)
        {
            var statusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError;
            return req.CreateResponse(statusCode);
        }
        catch
        {
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    [Function(nameof(StopBot))]
    public async Task<HttpResponseData> StopBot(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bot/stop")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            await Services.StopBotAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        catch (HttpRequestException ex)
        {
            var statusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError;
            return req.CreateResponse(statusCode);
        }
        catch
        {
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    [Function(nameof(GetBotDetail))]
    public async Task<HttpResponseData> GetBotDetail(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bot")] HttpRequestData req)
    {
        try
        {
            var botDetail = await Services.GetBotDetail();
            if (botDetail is null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(botDetail, HttpStatusCode.OK);

            return response;
        }
        catch (HttpRequestException ex)
        {
            var statusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError;
            return req.CreateResponse(statusCode);
        }
        catch
        {
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
