using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands {

    internal class ResetToSecond : AbstractCommand {

        public ResetToSecond()
            : base("сбросвх", "Сбросить пост вебхуков до последнего") {
        }

        public override bool Hidden
        {
            get
            {
                return true;
            }
        }

        [Inject]
        public DiscordWebHookPoster DiscordWebHookPoster
        {
            get;
            set;
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            DiscordWebHookPoster.ResetToSecond();
            await message.Channel.SendMessageAsync("Сброшено");
            return true;
        }
    }
}