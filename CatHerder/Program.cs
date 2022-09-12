using CatHerder;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceProvider serviceProvider = ConfigureServices();
Bot bot = serviceProvider.GetRequiredService<Bot>();
Task.WaitAny(bot.Start());

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