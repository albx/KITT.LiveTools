using Kitt.LiveTools.Shared.Models;
using System.Net.Http.Json;

namespace Kitt.LiveTools.Client.Services;

public class StreamingsHttpClient : IStreamingsClient
{
    public HttpClient Client { get; }

    public StreamingsHttpClient(HttpClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<IEnumerable<ScheduledStreaming>> GetScheduledStreamingsAsync()
    {
        var scheduledStreamings = await Client.GetFromJsonAsync<IEnumerable<ScheduledStreaming>>("api/streamings");
        return scheduledStreamings ?? Array.Empty<ScheduledStreaming>();
    }

    public async Task SaveStreamingStatsAsync(StreamingStats stats)
    {
        var response = await Client.PostAsJsonAsync("api/streamings/stats", stats);
        response.EnsureSuccessStatusCode();
    }
}
