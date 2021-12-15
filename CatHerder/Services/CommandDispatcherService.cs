using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CatHerder.Services
{
    /// <summary>
    /// Receive messages from servers and dispatch to command modules.
    /// </summary>
    /// <remarks>
    /// Based on https://docs.stillu.cc/guides/commands/intro.html.
    /// </remarks>
    internal class CommandDispatcherService : IService
    {
        public CommandDispatcherService(DiscordSocketClient client, CommandService commandService, IServiceProvider serviceProvider)
        {
            Client = client;
            CommandService = commandService;
            ServiceProvider = serviceProvider;

            Client.MessageReceived += MessageReceived;
            CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider).GetAwaiter().GetResult();
        }

        private async Task MessageReceived(SocketMessage socketMessage)
        {
            SocketUserMessage? socketUserMessage = socketMessage as SocketUserMessage;
            SocketCommandContext context = new SocketCommandContext(Client, socketUserMessage);
            if (socketUserMessage != null && !socketUserMessage.Author.IsBot)
            {
                MatchCollection matches = CommandParser.Matches(socketUserMessage.Content);
                if (matches.Any()
                    && ulong.TryParse(matches.First().Groups[1].Value, out ulong userId)
                    && Client.CurrentUser.Id == userId)
                {
                    await CommandService.ExecuteAsync(
                        context: context,
                        argPos: matches.First().Length,
                        services: ServiceProvider);
                }
                else if(socketUserMessage.Channel is IPrivateChannel)
                {
                    await context.Channel.SendMessageAsync("This bot does not respond to direct messages.");
                }
            }
        }
        
        private DiscordSocketClient Client { get; }
        private CommandService CommandService { get; }
        private IServiceProvider ServiceProvider { get; }

        // Support both via a user (!) and role (&)
        private readonly Regex CommandParser = new Regex(@"^<@[!&](\d+)>\s*!");
    }
}
