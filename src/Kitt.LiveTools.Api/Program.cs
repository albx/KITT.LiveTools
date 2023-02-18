using Kitt.LiveTools.Api.Configuration;
using Kitt.LiveTools.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.Configure<KittDatabaseConfiguration>(options =>
        {
            options.ConnectionString = context.Configuration.GetConnectionString("KittDatabase");
        });

        services.AddScoped<StreamingServices>();
        services.AddHttpClient<BotServices>(client =>
        {
            client.BaseAddress = new Uri(context.Configuration["BotEndpoint"]);

            var credentials = $"{context.Configuration["BotUsername"]}:{context.Configuration["BotPassword"]}";
            var authorizationValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authorizationValue);
        });
    })
    .Build();

host.Run();