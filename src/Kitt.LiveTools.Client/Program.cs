using Kitt.LiveTools.Client;
using Kitt.LiveTools.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IStreamingsClient, StreamingsHttpClient>(
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));


await builder.Build().RunAsync();