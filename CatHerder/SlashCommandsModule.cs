using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CatHerder
{
    public class SlashCommandsModule : InteractionModuleBase<InteractionContext>
    {
        [SlashCommand("permissions-report", "Send a report listing server users and their role memberships to the caller via direct message")]
        [RequireUserPermission(GuildPermission.ManageRoles | GuildPermission.ManageChannels)]
        public async Task PermissionsReport()
        {
            await Context.Interaction.DeferAsync(ephemeral: true);

            try
            {
                // Context.Guild is null for some reason
                SocketGuild guild = ((SocketGuildUser)Context.User).Guild;

                // Requires SERVER MEMBERS INTENT to be enabled for the bot. Otherwise,
                // this line will hang. 
                await guild.DownloadUsersAsync();

                IEnumerable<IRole> roles = guild.Roles.OrderBy(role => role.Position);

                using MemoryStream memoryStream = new MemoryStream();
                using StreamWriter streamWriter = new StreamWriter(memoryStream);
                streamWriter.WriteLine("User," + string.Join(",", roles.Select(role => EscapeForCSV(role.Name))));
                foreach ((string name, IEnumerable<IRole> userRoles) in guild.Users.Select(user => (name: EscapeForCSV(GetDisplayName(user)), userRoles: user.Roles))
                                                                                   .OrderBy(user => user.name))
                {
                    streamWriter.WriteLine(name + ","
                        + string.Join(",", roles.Select(role => userRoles.Any(userRole => userRole.Id == role.Id) ? "Y" : "N")));
                }
                streamWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                await Context.Interaction.FollowupWithFileAsync(
                    fileStream: memoryStream,
                    fileName: "permissions.csv",
                    text: "I attached the report.",
                    ephemeral: true
                );
            }
            catch
            {
                await Context.Interaction.ModifyOriginalResponseAsync(messageProperties => messageProperties.Content = "I have failed.");
                throw;
            }
        }

        public string EscapeForCSV(string text)
        {
            return text.Replace(',', '_');
        }

        public string GetDisplayName(IGuildUser user)
        {
            return user.Nickname ?? user.Username;
        }
    }
}