# Cat Herder

Cat Herder is a simple Discord bot to help administer the EDA Kunti League Discord server. 

Enter the slash command `/permssions-report` to generate a CSV. This lists each user in a row and each role in a column, showing their memberships. This command is only available to users with "Mmanage Channels"" and "Managed Roles" permissions.

## Overview

This repository automatically deploys to Azure on a successful build.

To install and run manually:
1. Create a Discord application and turn it into a bot at https://discord.com/developers/applications.
2. Add the new bot to the Discord server using https://discordapp.com/oauth2/authorize?client_id=Bot_Client_ID&scope=bot&permissions=268435456, replacing "Bot_Client_ID" with the bot's OAuth2 client ID. The bot needs at least the "Manage Roles" permission.
3. Authorize the bot for application commands by visiting https://discord.com/api/oauth2/authorize?client_id=Bot_Client_ID&scope=applications.commands, replacing "Bot_Client_ID" with the bot's OAuth2 client ID.
4. Place the API key for the bot in the DISCORD_APIKEY environment variable. You can find this on the "Bot" page under "Settings".
5. Ensure the "SERVER MEMBERS INTENT" intent is activated in the bot configuration. Otherwise, the code to process the command will hang.
6. Download, build then run the CatHerder project.

Log messages are written to the console. Press Escape to stop the bot.

The structure is simple and based off the examples in the Discord documentation. Key points:
1. Execution starts in Program.cs.
2. The Microsoft Dependency Injection framework initializes types. I may add configuration in the future.
3. The Bot class contains most of the logic.
4. Commands are implented in SlashCommandsModule, likely to be one command per class in the future.

## References

1. Discord API reference: https://discord.com/developers/docs/intro
2. Discord.Net API reference: https://docs.stillu.cc/api/index.html

## License

See [LICENSE](LICENSE) for the license (GPL v3).