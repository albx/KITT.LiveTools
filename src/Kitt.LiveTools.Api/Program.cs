using Kitt.LiveTools.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services
            .AddScoped<StreamingServices>()
            .AddScoped<BotServices>();
    })
    .Build();

host.Run();