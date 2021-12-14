using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatHerder
{
    public class CommandsModule: ModuleBase<SocketCommandContext>
    {
        [Command("PermissionsReport")]
        public async Task PermissionsReport()
        {
            await ReplyAsync("Coming soon!");
        }
    }
}
