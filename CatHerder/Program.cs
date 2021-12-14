using Discord.WebSocket;
using System.Linq;

const string ApiKeyName = "DISCORD_APIKEY";
string? apiKey = Environment.GetEnvironmentVariable(ApiKeyName);
if(string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException($"{ ApiKeyName } environment variable missing or empty");
}

using DiscordSocketClient client = new DiscordSocketClient();
try
{
    await client.LoginAsync(Discord.TokenType.Bot, apiKey, true);
}
catch (Exception ex)
{
    Console.Error.WriteLine("Error authenticating: " + ex);
    throw;
}

const long EdaKuntiLeagueGuildId = 141831692699566080;
RestGuild? edaKuntiLeagueGuild = (await client.GetGuildsAsync()).FirstOrDefault(guild => guild.Id == EdaKuntiLeagueGuildId);

client.GetUserAsync()

