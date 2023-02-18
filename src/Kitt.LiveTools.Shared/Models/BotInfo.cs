namespace Kitt.LiveTools.Shared.Models;

public record BotInfo
{
    public string Name { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public bool IsRunning => Status.Equals("Running", StringComparison.InvariantCultureIgnoreCase);
}
