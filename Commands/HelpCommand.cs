using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands {

    internal class HelpCommand : AbstractCommand {

        [Inject]
        public CommandManager CommandManager
        {
            get; set;
        }

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        public HelpCommand()
            : base("хелп", "Отображает эту справку") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            IDictionary<string, ICommand> commands = CommandManager.GetCommands();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("Доступные команды:");
            foreach (string key in commands.Keys) {
                ICommand command;
                commands.TryGetValue(key, out command);
                builder.AppendLine(String.Format("\t{0}{1} - {2}", ConfigurationManager.Config.Discord.CommandPrefix, key, command.GetDescription()));
            }
            await message.Channel.SendMessageAsync(builder.ToString());
            return true;
        }
    }
}