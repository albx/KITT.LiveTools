using Kitt.LiveTools.Client;
using Kitt.LiveTools.Client.Configuration;
using Kitt.LiveTools.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Azure.Functions.Authentication.WebAssembly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<BotConfiguration>(options =>
{
    options.Endpoint = builder.Configuration["BotConfiguration:Endpoint"] ?? string.Empty;
});

builder.Services.AddStaticWebAppsAuthentication();

builder.Services.AddHttpClient<IStreamingsClient, StreamingsHttpClient>(
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddHttpClient<IBotClient, BotClient>(
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));


await builder.Build().RunAsync();