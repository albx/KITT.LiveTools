namespace Kitt.LiveTools.Shared.Models;

public record StreamingStats
{
    public Guid StreamingId { get; init; }

    public int Viewers { get; init; }

    public int Subscribers { get; init; }

    public int UserJoinedNumber { get; init; }

    public int UserLeftNumber { get; init; }
}
