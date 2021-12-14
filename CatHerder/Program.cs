using CatHerder;
using CatHerder.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

using ServiceProvider serviceProvider = ConfigureServices();
// TODO: Load these via reflection then instantiate them all, e.g. serviceProvider.GetRequiredServices<IService>();
serviceProvider.GetRequiredService<CommandHandlerService>();
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
    while (!Console.KeyAvailable && Console.ReadKey(true).Key != ConsoleKey.Escape)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}