using Kitt.LiveTools.Shared.Models;
using System.Net.Http.Json;

namespace Kitt.LiveTools.Api.Services;

public class BotServices
{
    public HttpClient Client { get; }

    public BotServices(HttpClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task StartBotAsync()
    {
        var response = await Client.PostAsync("/api/continuouswebjobs/LemonBot/start", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task StopBotAsync()
    {
        var response = await Client.PostAsync("/api/continuouswebjobs/LemonBot/stop", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<BotInfo?> GetBotDetail()
    {
        var detail = await Client.GetFromJsonAsync<BotInfo>("/api/continuouswebjobs/LemonBot");
        if (detail is null)
        {
            return null;
        }

        return detail;
    }
}
