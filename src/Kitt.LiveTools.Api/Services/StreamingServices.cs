using Dapper;
using Kitt.LiveTools.Api.Configuration;
using Kitt.LiveTools.Shared.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace Kitt.LiveTools.Api.Services;

public class StreamingServices
{
    private readonly KittDatabaseConfiguration _databaseConfiguration;

    public StreamingServices(IOptions<KittDatabaseConfiguration> databaseConfigurationOptions)
    {
        _databaseConfiguration = databaseConfigurationOptions?.Value ?? throw new ArgumentNullException(nameof(databaseConfigurationOptions));
    }

    public async Task<IEnumerable<ScheduledStreaming>> GetScheduledStreamingsAsync()
    {
        using var connection = new SqlConnection(_databaseConfiguration.ConnectionString);
        connection.Open();

        var result = await connection.QueryAsync<ScheduledStreaming>(
            """
            SELECT TOP 5 c.Id as Id, c.Title as Title 
            FROM KITT_Contents c JOIN KITT_Streamings s ON c.Id=s.Id 
            WHERE s.ScheduleDate >= @ScheduleDate 
            ORDER BY s.ScheduleDate
            """,
            param: new { ScheduleDate = DateTime.Today });

        return result;
    }

    public async Task SaveStreamingStatsAsync(StreamingStats streamingStats)
    {
        using var connection = new SqlConnection(_databaseConfiguration.ConnectionString);
        connection.Open();

        await connection.ExecuteAsync(
            """
            INSERT INTO KITT_StreamingStats (Id, StreamingId, Viewers, Subscribers, UserJoinedNumber, UserLeftNumber)
            VALUES (@Id, @StreamingId, @Viewers, @Subscribers, @UserJoinedNumber, @UserLeftNumber)
            """,
            new
            {
                Id = Guid.NewGuid(),
                streamingStats.StreamingId,
                streamingStats.Viewers,
                streamingStats.Subscribers,
                streamingStats.UserJoinedNumber,
                streamingStats.UserLeftNumber
            });
    }
}
