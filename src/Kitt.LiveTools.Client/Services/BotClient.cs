using Kitt.LiveTools.Shared.Models;
using System.Net.Http.Json;

namespace Kitt.LiveTools.Client.Services;

public class BotClient : IBotClient
{
    public HttpClient Client { get; }

    public BotClient(HttpClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task StartBotAsync()
    {
        using var response = await Client.PostAsync("api/bot/start", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task StopBotAsync()
    {
        using var response = await Client.PostAsync("api/bot/stop", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<BotInfo> GetBotDetailAsync()
    {
        var detail = await Client.GetFromJsonAsync<BotInfo>("api/bot");
        if (detail is null)
        {
            throw new InvalidOperationException("BOT detail not found");
        }

        return detail;
    }
}
