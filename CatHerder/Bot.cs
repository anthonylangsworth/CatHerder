using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatHerder
{
    /// <summary>
    /// A Discord bot.
    /// </summary>
    internal class Bot
    {
        /// <summary>
        /// Construct a <see cref="Bot"/>.
        /// </summary>
        /// <param name="discordClient">
        /// The <see cref="DiscordSocketClient"/> to use.
        /// </param>
        public Bot(DiscordSocketClient discordClient, InteractionService interactionService, IServiceProvider serviceProvider)
        {
            Client = discordClient;
            InteractionService = interactionService;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Discord features the bot uses, which must be requested up front.
        /// </summary>
        public static GatewayIntents Intents => GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildMessages;

        /// <summary>
        /// The Discord client.
        /// </summary>
        internal DiscordSocketClient Client { get; }
        internal InteractionService InteractionService { get; }
        internal IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Start the bot.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The bot was already started or the required API key was not foind in the DISCORD_APIKEY environment variable.
        /// </exception>
        public async Task Start()
        {
            if (Client.ConnectionState == ConnectionState.Connected)
            {
                throw new InvalidOperationException("Already started");
            }

            Client.Log += LogAsync;
            InteractionService.Log += LogAsync;

            IEnumerable<ModuleInfo> modules = InteractionService.AddModulesAsync(Assembly.GetExecutingAssembly(), ServiceProvider).GetAwaiter().GetResult();
            Client.SlashCommandExecuted += Client_SlashCommandExecutedAsync;
            Client.GuildAvailable += Client_GuildAvailableAsync;

            // TODO: Load this from configuraiton or similar
            const string ApiKeyName = "DISCORD_APIKEY";
            string? apiKey = Environment.GetEnvironmentVariable(ApiKeyName);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                await LogAsync(new LogMessage(LogSeverity.Critical, "Startup", $"{ ApiKeyName } environment variable missing or empty"));
            }
            else
            {
                await Client.LoginAsync(TokenType.Bot, apiKey);
                await Client.StartAsync();
            }

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine($"[General/{message.Severity}] {message}");
            return Task.CompletedTask;
        }

        private async Task Client_GuildAvailableAsync(SocketGuild guild)
        {
            await InteractionService.RegisterCommandsToGuildAsync(guild.Id);
        }

        private async Task Client_SlashCommandExecutedAsync(SocketSlashCommand socketSlashCommand)
        {
            await InteractionService.ExecuteCommandAsync(
                new InteractionContext(Client, socketSlashCommand, socketSlashCommand.User),
                ServiceProvider);
        }
    }
}
