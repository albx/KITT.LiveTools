using Kitt.LiveTools.Shared.Models;

namespace Kitt.LiveTools.Client.Services;

public interface IStreamingsClient
{
    Task<IEnumerable<ScheduledStreaming>> GetScheduledStreamingsAsync();

    Task SaveStreamingStatsAsync(StreamingStats stats);
}
