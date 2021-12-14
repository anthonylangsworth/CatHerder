using Discord.Rest;

const string ApiKeyName = "DISCORD_APIKEY";
string? apiKey = Environment.GetEnvironmentVariable(ApiKeyName);
if(apiKey == null)
{
    throw new InvalidOperationException($"{ ApiKeyName } environment variable missing or empty");
}

using DiscordRestClient client = new DiscordRestClient();
try
{
    await client.LoginAsync(Discord.TokenType.Bot, apiKey, true);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);
}

IReadOnlyCollection<RestGuild> guilds = await client.GetGuildsAsync();
Console.WriteLine($"{ guilds.Count } servers");

