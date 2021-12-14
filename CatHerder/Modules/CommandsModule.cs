using Discord;
using Discord.Commands;
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
        public async Task PermissionsReport()
        {
            IReadOnlyCollection<SocketRole> roles = Context.Guild.Roles;

            using MemoryStream memoryStream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.WriteLine("User," + string.Join(",", roles.Select(role => role.Name)));
            foreach (SocketGuildUser user in Context.Guild.Users)
            {
                streamWriter.WriteLine(user.Nickname + "," + string.Join(",", roles.Select(role => user.Roles.Any(r => r.Id == role.Id) ? "Y" : "N")));
            }
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            await Context.Channel.SendFileAsync(memoryStream, "permissions.csv");
        }
    }
}