using Kitt.LiveTools.Shared.Models;

namespace Kitt.LiveTools.Client.Services;

public class StreamingsHttpClient : IStreamingsClient
{
    public HttpClient Client { get; }

    public StreamingsHttpClient(HttpClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public Task<IEnumerable<ScheduledStreaming>> GetScheduledStreamingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveStreamingStatsAsync(StreamingStats stats)
    {
        throw new NotImplementedException();
    }
}
