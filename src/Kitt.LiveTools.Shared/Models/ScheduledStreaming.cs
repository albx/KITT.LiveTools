namespace Kitt.LiveTools.Shared.Models;

public record ScheduledStreaming
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;
}
