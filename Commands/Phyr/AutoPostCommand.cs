﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using InstaSharp.Models;
using JuniperBot.Model;
using JuniperBot.Model.Events;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands.Phyr {

    internal class AutoPostCommand : PostCommand {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(AutoPostCommand));

        [Inject]
        public InstagramPoller InstagramPoller
        {
            get; set;
        }

        private List<BotContext> Contexts = new List<BotContext>();

        private bool Initialized;

        public AutoPostCommand()
            : base("нафыркивай", "Автоматически нафыркивать новые посты из блога Джупи") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            bool added = false;
            lock (Contexts) {
                if (!Contexts.Contains(context)) {
                    added = true;
                    Contexts.Add(context);
                    if (!Initialized) {
                        InstagramPoller.Update += InstagramPoller_Update;
                        Initialized = true;
                    }
                }
            }
            if (!added) {
                await context.Channel.SendMessageAsync("Ты меня уже просил нафыркивать!");
                return true;
            } else {
                await context.Channel.SendMessageAsync("Хорошо! Как только будет что-то новенькое я сюда фыркну ^_^");
            }

            return true;
        }

        private async void InstagramPoller_Update(object sender, UpdateInstagramEventArgs e) {
            foreach (BotContext context in Contexts) {
                if (context.LatestId != null) {
                    List<Media> newMedias = new List<Media>();
                    IEnumerator<Media> enumerator = e.Medias.GetEnumerator();
                    while (enumerator.MoveNext() && !context.LatestId.Equals(enumerator.Current.Id)) {
                        newMedias.Add(enumerator.Current);
                    }

                    if (newMedias.Count > 0) {
                        await Post(newMedias, context);
                    }
                }
                context.LatestId = e.Medias[0].Id;
            }
        }
    }
}