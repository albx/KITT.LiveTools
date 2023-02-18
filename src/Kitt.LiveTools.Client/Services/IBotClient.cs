using Kitt.LiveTools.Shared.Models;

namespace Kitt.LiveTools.Client.Services;

public interface IBotClient
{
    Task StartBotAsync();

    Task StopBotAsync();

    Task<BotInfo> GetBotDetailAsync();
}
