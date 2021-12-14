using CatHerder;
using CatHerder.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using ServiceProvider serviceProvider = ConfigureServices();
serviceProvider.GetServices<IService>(); // Instantiate services
Bot bot = serviceProvider.GetRequiredService<Bot>();
Task.WaitAny(bot.Start(), ReadKeys());

ServiceProvider ConfigureServices()
{
    IServiceCollection serviceCollection = new ServiceCollection()
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<Bot>()
        .AddSingleton<CommandService>();

    // Add services
    foreach(Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IService))))
    {
        serviceCollection.AddSingleton(typeof(IService), type);
    }
    
    return serviceCollection.BuildServiceProvider();
}

async Task ReadKeys()
{ 
    // Reference: https://stackoverflow.com/questions/5891538/listen-for-key-press-in-net-console-app
    while (!Console.KeyAvailable && Console.ReadKey(true).Key != ConsoleKey.Escape)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}