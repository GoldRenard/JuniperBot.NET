using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using InstaSharp.Models;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands {

    internal class PhyrCommand : AbstractCommand {

        [Inject]
        public InstagramClient InstagramClient
        {
            get; set;
        }

        public PhyrCommand()
            : base("фыр", "публикация последнего поста из блога Джупи") {
        }

        public async override Task<bool> DoCommand(DiscordMessage message, string[] args) {
            List<Media> medias = await InstagramClient.GetRecent();
            if (medias != null) {
                List<DiscordEmbed> result = new List<DiscordEmbed>();
                if (medias.Count > 0) {
                    DiscordEmbed embed = ConvertToEmbed(medias[0]);
                    await message.RespondAsync(embed.Title, false, embed);
                }
            }
            return true;
        }

        public static DiscordEmbed ConvertToEmbed(Media media) {
            return new DiscordEmbed() {
                Author = new DiscordEmbedAuthor() {
                    IconUrl = media.User.ProfilePicture,
                    Name = media.User.FullName
                },

                Image = new DiscordEmbedImage() {
                    Url = media.Images.StandardResolution.Url
                },

                Thumbnail = new DiscordEmbedThumbnail() {
                    Url = media.Images.Thumbnail.Url
                },
                Timestamp = media.CreatedTime,
                Title = media.Caption.Text,
                Url = media.Link,
            };
        }
    }
}