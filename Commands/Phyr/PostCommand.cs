using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using InstaSharp.Models;
using JuniperBot.Model;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands.Phyr {

    internal class PostCommand : AbstractCommand {

        [Inject]
        public InstagramClient InstagramClient
        {
            get; set;
        }

        public PostCommand()
            : base("фыр", "Фыркнуть посты из блога Джупи") {
        }

        public async override Task<bool> DoCommand(DiscordMessage message, BotContext context, string[] args) {
            List<Media> medias = await InstagramClient.GetRecent();
            if (medias != null) {
                List<DiscordEmbed> result = new List<DiscordEmbed>();
                if (medias.Count > 0) {
                    DiscordEmbed embed = ConvertToEmbed(context, medias[0]);
                    await message.RespondAsync("", false, embed);
                }
            }

            return true;
        }

        public static DiscordEmbed ConvertToEmbed(BotContext context, Media media) {
            DiscordEmbed embed = new DiscordEmbed() {
                Image = new DiscordEmbedImage() {
                    Url = media.Images.StandardResolution.Url
                }
            };

            if (context.DetailedEmbed) {
                embed.Author = new DiscordEmbedAuthor() {
                    IconUrl = media.User.ProfilePicture,
                    Name = media.User.FullName
                };
                embed.Timestamp = media.CreatedTime;
                embed.Url = media.Link;
                if (media.Caption != null) {
                    if (!string.IsNullOrEmpty(media.Caption.Text)) {
                        embed.Title = media.Caption.Text;
                    }
                }
            }
            return embed;
        }
    }
}