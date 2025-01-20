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
                                                                     .Build();
    return new ServiceCollection()
        .AddSingleton<IConfiguration>(configurationRoot)
        .AddSingleton(sp => new DiscordSocketConfig()
        {
            GatewayIntents = Bot.Intents,
            UseInteractionSnowflakeDate = false // Prevent clock sync issues
        })
        .AddSingleton(sp => new DiscordSocketClient(sp.GetRequiredService<DiscordSocketConfig>()))
        .AddSingleton(sp => new InteractionService(sp.GetRequiredService<DiscordSocketClient>()))
        .AddSingleton<Bot>()
        .BuildServiceProvider();
}