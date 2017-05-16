using System.Collections.Generic;
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
        public DiscordClient DiscordClient
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
            embedBuilder.ThumbnailUrl = DiscordClient.Client.CurrentUser.GetAvatarUrl();
            foreach (string key in commands.Keys) {
                ICommand command;
                commands.TryGetValue(key, out command);
                if (!command.Hidden) {
                    embedBuilder.AddInlineField(ConfigurationManager.Config.Discord.CommandPrefix + key, command.GetDescription());
                }
            }
            await message.Channel.SendMessageAsync("**Доступные команды:**", false, embedBuilder.Build());
            return true;
        }
    }
}