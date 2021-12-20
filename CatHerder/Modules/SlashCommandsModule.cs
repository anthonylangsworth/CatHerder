using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.Webhook;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatHerder.Modules
{
    public class SlashCommandsModule: InteractionModuleBase<InteractionContext>
    {
        [SlashCommand("permissions-report", "Construct a report listing server users and their role memberships")]
        //[RequireUserPermission(GuildPermission.ManageRoles | GuildPermission.ManageChannels,
        //    ErrorMessage = "You lack required permissions to request this report.")]
        public async Task PermissionsReport()
        {
            await Context.Interaction.DeferAsync();

            try
            {
                // Requires SERVER MEMBERS INTENT to be enabled for the bot. Otherwise,
                // this line will hang. 
                await Context.Guild.DownloadUsersAsync();
                    
                IEnumerable<IRole> roles = Context.Guild.Roles.OrderBy(role => role.Position);

                using MemoryStream memoryStream = new MemoryStream();
                using StreamWriter streamWriter = new StreamWriter(memoryStream);
                streamWriter.WriteLine("User," + string.Join(",", roles.Select(role => EscapeForCSV(role.Name))));
                foreach (IGuildUser user in (await Context.Guild.GetUsersAsync()).OrderBy(user => user.Nickname ?? user.Username))
                {
                    streamWriter.WriteLine(EscapeForCSV(user.Nickname ?? user.Username) + ","
                        + string.Join(",", roles.Select(role => user.RoleIds.Any(roleId => roleId == role.Id) ? "Y" : "N")));
                }
                streamWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                IDMChannel dmChannel = await Context.User.CreateDMChannelAsync();
                try
                {
                    await dmChannel.SendFileAsync(
                        stream: memoryStream,
                        filename: "permissions.csv",
                        text: "I have attached a report listing the server users and their role memberships.");
                }
                finally
                {
                    await dmChannel.CloseAsync();
                }

                await Context.Interaction.ModifyOriginalResponseAsync(messageProperties => messageProperties.Content = "I have responded via direct message.");
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
    }
}