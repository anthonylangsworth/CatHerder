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
        /// <summary>
        /// Construct a <see cref="CommandDispatcherService"/>.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="commandService"></param>
        /// <param name="serviceProvider"></param>
        public CommandDispatcherService(DiscordSocketClient client, CommandService commandService, IServiceProvider serviceProvider)
        {
            Client = client;
            CommandService = commandService;
            ServiceProvider = serviceProvider;

            Client.MessageReceived += MessageReceived;
            CommandService.AddModulesAsync(Assembly.GetExecutingAssembly(), ServiceProvider).GetAwaiter().GetResult();
        }

        private async Task MessageReceived(SocketMessage socketMessage)
        {
            SocketUserMessage? socketUserMessage = socketMessage as SocketUserMessage;
            SocketCommandContext context = new SocketCommandContext(Client, socketUserMessage);
            if (socketUserMessage != null && !socketUserMessage.Author.IsBot)
            {
                if (socketUserMessage.Channel is IPrivateChannel)
                {
                    await context.Channel.SendMessageAsync(
                        "I cannot determine your server via direct messages. Give me commands using a mention on a server channel.");
                }
                else
                {
                    (bool IsToMe, int argPos) = IsToUser(socketUserMessage.Content, Client.CurrentUser.Id);
                    if (IsToMe)
                    {
                        await CommandService.ExecuteAsync(context, argPos, ServiceProvider);
                    }
                }
            }
        }

        /// <summary>
        /// Is the message to the given user, either directly or via "@role"?
        /// </summary>
        /// <param name="content"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static (bool IsToMe, int argPos) IsToUser(string content, ulong userId)
        {
            (bool IsToMe, int argPos) result;
            MatchCollection matches = CommandParser.Matches(content);
            if (matches.Any())
            {
                result = (
                    ulong.TryParse(matches.First().Groups[1].Value, out ulong parsedUserId)
                        && userId == parsedUserId,
                    matches.First().Length
                );
            }
            else
            {
                result = (false, 0);
            }
            return result;
        }

        private DiscordSocketClient Client { get; }
        private CommandService CommandService { get; }
        private IServiceProvider ServiceProvider { get; }

        // Support both via a user (!) and role (&)
        private static readonly Regex CommandParser = new Regex(@"^<@[!&](\d+)>\s*!");
    }
}
