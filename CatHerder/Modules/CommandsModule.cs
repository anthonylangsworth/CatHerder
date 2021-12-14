using Discord;
using Discord.Commands;
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
    public class CommandsModule: ModuleBase<SocketCommandContext>
    {
        [Command("PermissionsReport")]
        [RequireUserPermission(ChannelPermission.ManageRoles | ChannelPermission.ManageChannels)]
        public async Task PermissionsReport()
        {
            // Requires SERVER MEMBERS INTENT to be enabled for the bot. Otherwise,
            // this line will hang.
            await Context.Client.DownloadUsersAsync(new[] { Context.Guild });

            IEnumerable<SocketRole> roles = Context.Guild.Roles.OrderBy(role => role.Position);

            using MemoryStream memoryStream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.WriteLine("User," + string.Join(",", roles.Select(role => EscapeForCSV(role.Name))));
            foreach (SocketGuildUser user in Context.Guild.Users.OrderBy(user => user.Nickname ?? user.Username))
            {
                streamWriter.WriteLine(EscapeForCSV(user.Nickname ?? user.Username) + ","
                    + string.Join(",", roles.Select(role => user.Roles.Any(r => r.Id == role.Id) ? "Y" : "N")));
            }
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            await Context.Channel.SendFileAsync(memoryStream, "permissions.csv");
        }

        public string EscapeForCSV(string text)
        {
            return text.Replace(',', '_');
        }
    }
}