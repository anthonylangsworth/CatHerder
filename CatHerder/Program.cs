using CatHerder;
using CatHerder.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

//const long EdaKuntiLeagueGuildId = 141831692699566080;
//SocketGuild edaKuntiLeagueGuild = client.GetGuild(EdaKuntiLeagueGuildId);

using ServiceProvider serviceProvider = ConfigureServices();
await serviceProvider.GetRequiredService<CommandHandlerService>().InstallCommandsAsync();
serviceProvider.GetRequiredService<LoggingService>();
Bot bot = serviceProvider.GetRequiredService<Bot>();
Task.WaitAny(bot.Start(), ReadKeys());

ServiceProvider ConfigureServices()
{
    ServiceProvider serviceProvider = new ServiceCollection()
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<Bot>()
        .AddSingleton<CommandService>()
        .AddSingleton<CommandHandlerService>()
        .AddSingleton<LoggingService>()
        .BuildServiceProvider();

    return serviceProvider;
}

async Task ReadKeys()
{
    // Reference: https://stackoverflow.com/questions/5891538/listen-for-key-press-in-net-console-app
    Console.WriteLine("Press ESC to end.");
    while (!Console.KeyAvailable && Console.ReadKey(true).Key != ConsoleKey.Escape)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}