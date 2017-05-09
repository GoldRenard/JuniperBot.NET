using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using InstaSharp.Models;
using JuniperBot.Model;
using JuniperBot.Model.Exception;
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
            : base("фыр", "Фыркнуть посты из блога Джупи (можно указать количество постов, по-умолчанию одно)") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            int count = ParseCount(context, args);

            List<Media> medias = await InstagramClient.GetRecent();
            if (medias.Count == 0) {
                await message.Channel.SendMessageAsync("Что-то мне и нечего показать...");
                return true;
            }

            if (count > medias.Count) {
                await message.Channel.SendMessageAsync($"У меня есть всего {medias.Count} сообщений...");
                count = medias.Count;
            }

            medias = medias.GetRange(0, count);
            if (medias.Count > 0) {
                if (context.DetailedEmbed) {
                    foreach (Media media in medias) {
                        Embed embed = ConvertToEmbed(context, media);
                        await message.Channel.SendMessageAsync("", false, embed);
                    }
                } else {
                    IEnumerator<Media> iterator = medias.GetEnumerator();
                    List<string> messages = new List<string>();
                    StringBuilder builder = new StringBuilder();
                    while (iterator.MoveNext()) {
                        Media media = iterator.Current;
                        string newEntry = media.Images.StandardResolution.Url + '\n';
                        if (builder.Length + newEntry.Length > MAX_MESSAGE_SIZE) {
                            messages.Add(builder.ToString());
                            builder.Clear();
                        }
                        builder.Append(newEntry);
                    }
                    if (builder.Length > 0) {
                        messages.Add(builder.ToString());
                    }
                    foreach (string part in messages) {
                        await message.Channel.SendMessageAsync(part);
                        await Task.Delay(2000);
                    }
                }
            }
            return true;
        }

        private static int ParseCount(BotContext context, string[] args) {
            int count = 1;
            if (args.Length > 0) {
                try {
                    count = int.Parse(args[0]);
                } catch (Exception e) {
                    throw new ValidationException("Фыр на тебя. Число мне, число!");
                }

                if (count == 0) {
                    throw new ValidationException("Всмысле ноль? Ну ладно, не буду ничего присылать.");
                }

                if (context.DetailedEmbed) {
                    if (count > 3) {
                        throw new ValidationException("Не могу прислать больше 3 фырок в нефырном виде :C");
                    }
                } else {
                    if (count > 20) {
                        throw new ValidationException("Не могу прислать больше 20 фырок за раз :C");
                    }
                }

                if (count < 0) {
                    throw new ValidationException("Фтооо ты хочешь от меня?");
                }
            }
            return count;
        }

        public static Embed ConvertToEmbed(BotContext context, Media media) {
            EmbedBuilder builder = new EmbedBuilder() {
                ImageUrl = media.Images.StandardResolution.Url
            };

            if (context.DetailedEmbed) {
                builder.Author = new EmbedAuthorBuilder();
                builder.Author.IconUrl = media.User.ProfilePicture;
                builder.Author.Name = media.User.FullName;
                builder.Timestamp = media.CreatedTime;
                builder.Url = media.Link;
                if (media.Caption != null) {
                    if (!string.IsNullOrEmpty(media.Caption.Text)) {
                        builder.Title = media.Caption.Text;
                    }
                }
            }
            return builder.Build();
        }
    }
}