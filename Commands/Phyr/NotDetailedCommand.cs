using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;

namespace JuniperBot.Commands.Phyr {

    internal class NotDetailedCommand : AbstractCommand {

        public NotDetailedCommand()
            : base("фырно", "Фырчать фырные картинки без лишних нефыров") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            if (!context.DetailedEmbed) {
                await message.Channel.SendMessageAsync("Уже фырнее некуда! ^_^");
                return true;
            }
            context.DetailedEmbed = false;
            await message.Channel.SendMessageAsync("Пофырчим! <^.^>");
            return true;
        }
    }
}