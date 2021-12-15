# Cat Herder

Cat Herder is a simple Discord bot to help administer the EDA Kunti League Discord server. 

## Overview

To install and run:
1. Create a Discord application and turn it into a bot at https://discord.com/developers/applications.
2. Add the new bot to the Discord server using https://discordapp.com/oauth2/authorize?client_id=Bot_Client_ID&scope=bot&permissions=268435456, replacing "Bot_Client_ID" with the bot's client ID. The bot needs at least the "Manage Roles" permission. 
3. Place the API key for the bot in the DISCORD_APIKEY environment variable.
4. Ensure the "SERVER MEMBERS INTENT" intent is activated in the bot configuration. Otherwise, the code to process the command will hang.
5. Download, build then run the CatHerder project.

Log messages are written to the console. Press Escape to stop the bot.

The structure is simple and based off the examples in the Discord documentation. Key points:
1. Execution starts in Program.cs.
2. The Microsoft Dependency Injection framework initializes types. I may add configuration in the future.
3. The Services folder includes classes that augment or modify behaviour. This includes a command dispatcher (called a handler in the Discord.Net documentation) and a logger.
4. The Modules folder contains the command implementations.

## References

1. Discord API reference: https://discord.com/developers/docs/intro
2. Discord.Net API reference: https://docs.stillu.cc/api/index.html
