using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;

namespace JuniperBot.Commands.Phyr {

    internal class DetailedCommand : AbstractCommand {

        public DetailedCommand()
            : base("нефырно", "Фырчать фырные картинки с непонятными надписями") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            if (context.DetailedEmbed) {
                await message.Channel.SendMessageAsync("Тебе мало буков? >_>");
                return true;
            }
            context.DetailedEmbed = true;
            await message.Channel.SendMessageAsync("Ну фыыыр :C");
            return true;
        }
    }
}