using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Bot(DiscordSocketClient discordClient)
        {
            Client = discordClient;
        }

        /// <summary>
        /// Start the bot.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The required API key was not foind in the DISCORD_APIKEY environment variable.
        /// </exception>
        public async Task Start()
        {
            // TODO: Load this from configuraiton or similar
            const string ApiKeyName = "DISCORD_APIKEY";
            string? apiKey = Environment.GetEnvironmentVariable(ApiKeyName);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException($"{ ApiKeyName } environment variable missing or empty");
            }

            await Client.LoginAsync(TokenType.Bot, apiKey);
            await Client.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        /// <summary>
        /// The Discord client.
        /// </summary>
        internal DiscordSocketClient Client { get; }
    }
}
