using Kitt.LiveTools.Client.Configuration;
using Kitt.LiveTools.Client.Services;
using Kitt.LiveTools.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Kitt.LiveTools.Client.Components;

public partial class Management : IAsyncDisposable
{
    private ViewModel model = new();

    private BotInfo? botDetail = null;

    private DateTime? botStartTime = null;

    private IEnumerable<ScheduledStreaming> scheduledStreamings = Array.Empty<ScheduledStreaming>();

    private HubConnection? connection = null;

    [Inject]
    public IStreamingsClient StreamingsClient { get; set; } = default!;

    [Inject]
    public IBotClient BotClient { get; set; } = default!;

    [Inject]
    public IOptions<BotConfiguration> BotConfigurationOptions { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        scheduledStreamings = await StreamingsClient.GetScheduledStreamingsAsync();
        if (scheduledStreamings.Any())
        {
            model.StreamingId = scheduledStreamings.First().Id;
        }

        botDetail = await BotClient.GetBotDetailAsync();

        await InitializeSignalrAsync();
    }

    private async Task InitializeSignalrAsync()
    {
        try
        {
            connection = new HubConnectionBuilder()
                .WithUrl(BotConfigurationOptions.Value.Endpoint)
                .Build();

            connection.On<StartNotification>("BotStarted", OnBotStartedHandler);
            connection.On<StopNotification>("BotStopped", OnBotStoppedHandler);
            connection.On<string>("UserSubscriptionReceived", OnUserSubscriptionReceivedHandler);
            connection.On<string>("UserLeftReceived", OnUserLeftHandler);
            connection.On<string>("UserJoinReceived", OnUserJoinedHandler);

            await connection.StartAsync();
        }
        catch
        {
            //
        }
    }

    private void OnUserJoinedHandler(string userName)
    {
        model.UserJoinedNumber += 1;
        if (!model.Viewers.Contains(userName))
        {
            model.Viewers.Add(userName);
        }

        StateHasChanged();
    }

    private void OnUserLeftHandler(string userName)
    {
        model.UserLeftNumber += 1;
        if (model.Viewers.Contains(userName))
        {
            model.Viewers.Remove(userName);
        }

        StateHasChanged();
    }

    private void OnUserSubscriptionReceivedHandler(string subscriberName)
    {
        model.Subscribers += 1;
        StateHasChanged();
    }

    private void OnBotStartedHandler(StartNotification notification)
    {
        botDetail = (botDetail ?? new()) with { Status = "Running" };
        botStartTime = notification.StartTime;

        StateHasChanged();
    }

    private void OnBotStoppedHandler(StopNotification notification)
    {
        botDetail = (botDetail ?? new()) with { Status = "Stopped" };
        botStartTime = null;

        StateHasChanged();
    }

    private async Task SaveStreamingStatsAsync()
    {
        var stats = new StreamingStats
        {
            StreamingId = model.StreamingId!.Value,
            Subscribers = model.Subscribers,
            UserJoinedNumber = model.UserJoinedNumber,
            UserLeftNumber = model.UserLeftNumber,
            Viewers = model.Viewers.Count,
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

    #region IAsyncDisposable
    public async ValueTask DisposeAsync()
    {
        if (connection is not null)
        {
            await connection.DisposeAsync();
        }
    }
    #endregion

    public class ViewModel
    {
        [Required]
        public Guid? StreamingId { get; set; }

        public HashSet<string> Viewers { get; set; } = new HashSet<string>();

        public int Subscribers { get; set; }

        public int UserJoinedNumber { get; set; }

        public int UserLeftNumber { get; set; }
    }

    public record StartNotification
    {
        public DateTime StartTime { get; init; }
    }

    public record StopNotification
    {
        public DateTime StopTime { get; init; }
    }
}
