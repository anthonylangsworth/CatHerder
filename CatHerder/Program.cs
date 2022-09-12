using CatHerder;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceProvider serviceProvider = ConfigureServices();
await serviceProvider.GetRequiredService<Bot>().Start();

static ServiceProvider ConfigureServices()
{
    IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddEnvironmentVariables("")
                                                                     // .AddAzureAppConfiguration()
                                                                     .Build();
    return new ServiceCollection()
        // .AddAzureAppConfiguration()
        .AddSingleton<IConfiguration>(configurationRoot)
        .AddSingleton(sp => new DiscordSocketConfig()
        {
            GatewayIntents = Bot.Intents
        })
        .AddSingleton(sp => new DiscordSocketClient(sp.GetRequiredService<DiscordSocketConfig>()))
        .AddSingleton<InteractionService>()
        .AddSingleton<Bot>()
        .BuildServiceProvider();
}