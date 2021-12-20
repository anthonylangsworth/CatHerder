using CatHerder;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using ServiceProvider serviceProvider = ConfigureServices();
Bot bot = serviceProvider.GetRequiredService<Bot>();
Task.WaitAny(bot.Start(), ReadKeys());

ServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddSingleton(sp => new DiscordSocketConfig()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = Bot.Intents
        })
        .AddSingleton(sp => new DiscordSocketClient(sp.GetRequiredService<DiscordSocketConfig>()))
        .AddSingleton<InteractionService>()
        .AddSingleton<Bot>()
        .BuildServiceProvider();
}

async Task ReadKeys()
{ 
    // Reference: https://stackoverflow.com/questions/5891538/listen-for-key-press-in-net-console-app
    while (!Console.KeyAvailable && Console.ReadKey(true).Key != ConsoleKey.Escape)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}