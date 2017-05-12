using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
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
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.ThumbnailUrl = message.Discord.CurrentUser.GetAvatarUrl();
            foreach (string key in commands.Keys) {
                ICommand command;
                commands.TryGetValue(key, out command);
                embedBuilder.AddInlineField(ConfigurationManager.Config.Discord.CommandPrefix + key, command.GetDescription());
            }
            await message.Channel.SendMessageAsync("**Доступные команды:**", false, embedBuilder.Build());
            return true;
        }
    }
}