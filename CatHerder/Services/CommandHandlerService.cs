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
    /// 
    /// </summary>
    /// <remarks>
    /// Base on https://docs.stillu.cc/guides/commands/intro.html.
    /// </remarks>
    internal class CommandHandlerService
    {
        public CommandHandlerService(DiscordSocketClient client, CommandService commandService, IServiceProvider serviceProvider)
        {
            Client = client;
            CommandService = commandService;
            ServiceProvider = serviceProvider;

            Client.MessageReceived += HandleCommandAsync;
            CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider).GetAwaiter().GetResult();
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            SocketUserMessage? socketUserMessage = socketMessage as SocketUserMessage;
            if (socketUserMessage != null && !socketUserMessage.Author.IsBot)
            {
                MatchCollection matches = CommandParser.Matches(socketUserMessage.Content);
                if (matches.Any()
                    && ulong.TryParse(matches.First().Groups[1].Value, out ulong userId)
                    && Client.CurrentUser.Id == userId)
                {
                    await CommandService.ExecuteAsync(
                        context: new SocketCommandContext(Client, socketUserMessage),
                        argPos: matches.First().Length,
                        services: ServiceProvider);
                }
            }
        }

        private DiscordSocketClient Client { get; }
        private CommandService CommandService { get; }
        private IServiceProvider ServiceProvider { get; }

        // Support both via a user (!) and role (@)
        private readonly Regex CommandParser = new Regex(@"^<@[!&](\d+)>\s*!");
    }
}
