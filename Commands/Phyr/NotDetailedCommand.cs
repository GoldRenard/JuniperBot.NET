﻿using System.Threading.Tasks;
using DSharpPlus;
using JuniperBot.Model;

namespace JuniperBot.Commands.Phyr {

    internal class NotDetailedCommand : AbstractCommand {

        public NotDetailedCommand()
            : base("фырно", "Фырчать фырные картинки без лишних нефыров") {
        }

        public async override Task<bool> DoCommand(DiscordMessage message, BotContext context, string[] args) {
            if (!context.DetailedEmbed) {
                await message.RespondAsync("Уже фырнее некуда! ^_^");
                return true;
            }
            context.DetailedEmbed = false;
            await message.RespondAsync("Пофырчим! <^.^>");
            return true;
        }
    }
}