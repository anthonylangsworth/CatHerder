using Discord.Commands;
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
            //const long EdaKuntiLeagueGuildId = 141831692699566080;
            //SocketGuild edaKuntiLeagueGuild = client.GetGuild(EdaKuntiLeagueGuildId);

            await ReplyAsync("Coming soon!");
        }
    }
}
