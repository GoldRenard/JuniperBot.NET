using System.Threading.Tasks;
using DSharpPlus;
using JuniperBot.Model;

namespace JuniperBot.Commands.Phyr {

    internal class DetailedCommand : AbstractCommand {

        public DetailedCommand()
            : base("нефырно", "Фырчать фырные картинки с непонятными надписями") {
        }

        public async override Task<bool> DoCommand(DiscordMessage message, BotContext context, string[] args) {
            if (context.DetailedEmbed) {
                await message.RespondAsync("Тебе мало буков? >_>");
                return true;
            }
            context.DetailedEmbed = true;
            await message.RespondAsync("Ну фыыыр :C");
            return true;
        }
    }
}