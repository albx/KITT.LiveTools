using Kitt.LiveTools.Api.Configuration;
using Kitt.LiveTools.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.Configure<KittDatabaseConfiguration>(options =>
        {
            options.ConnectionString = context.Configuration.GetConnectionString("KittDatabase");
        });

        services
            .AddScoped<StreamingServices>()
            .AddScoped<BotServices>();
    })
    .Build();

host.Run();