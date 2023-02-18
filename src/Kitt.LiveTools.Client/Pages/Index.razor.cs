using Kitt.LiveTools.Client.Services;
using Kitt.LiveTools.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Kitt.LiveTools.Client.Pages;

public partial class Index
{
    private ViewModel model = new();

    private BotInfo? botDetail = null;

    private DateTime? botStartTime = null;

    private IEnumerable<ScheduledStreaming> scheduledStreamings = Array.Empty<ScheduledStreaming>();

    [Inject]
    public IStreamingsClient StreamingsClient { get; set; } = default!;

    [Inject]
    public IBotClient BotClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        scheduledStreamings = await StreamingsClient.GetScheduledStreamingsAsync();
        if (scheduledStreamings.Any())
        {
            model.StreamingId = scheduledStreamings.First().Id;
        }

        botDetail = await BotClient.GetBotDetailAsync();

        //setup signalr connection -> subscribe to bot start / stop notification
        //start signalr connection
    }

    private async Task SaveStreamingStatsAsync()
    {
        var stats = new StreamingStats
        {
            StreamingId = model.StreamingId!.Value,
            Subscribers = model.Subscribers,
            UserJoinedNumber = model.UserJoinedNumber,
            UserLeftNumber = model.UserLeftNumber,
            Viewers = model.Viewers,
        };

        try
        {
            await StreamingsClient.SaveStreamingStatsAsync(stats);
        }
        catch
        {

        }
    }

    private async Task StartBotAsync()
    {
        await BotClient.StartBotAsync();
    }

    private async Task StopBotAsync()
    {
        await BotClient.StopBotAsync();
    }

    public class ViewModel
    {
        [Required]
        public Guid? StreamingId { get; set; }

        public int Viewers { get; set; }

        public int Subscribers { get; set; }

        public int UserJoinedNumber { get; set; }

        public int UserLeftNumber { get; set; }
    }
}
