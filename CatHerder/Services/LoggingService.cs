using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatHerder.Services
{
	/// <summary>
	/// Write log events to the console.
	/// </summary>
	/// <remarks>
	/// Copied from https://docs.stillu.cc/guides/concepts/logging.html.
	/// </remarks>
	internal class LoggingService : IService
	{
		public LoggingService(DiscordSocketClient client, CommandService command)
		{
			client.Log += LogAsync;
			command.Log += LogAsync;
		}

		private Task LogAsync(LogMessage message)
		{
			if (message.Exception is CommandException commandException)
			{
				Console.WriteLine($"[Command/{message.Severity}] {commandException.Command.Aliases.First()}"
					+ $" failed to execute in {commandException.Context.Channel}.");
				Console.WriteLine(commandException);
			}
			else
			{
				Console.WriteLine($"[General/{message.Severity}] {message}");
			}

			return Task.CompletedTask;
		}
	}
}
