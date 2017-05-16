using System;
using System.Collections.Generic;
using Discord;
using Discord.Webhook;
using InstaSharp.Models;
using JuniperBot.Commands.Phyr;
using JuniperBot.Model.Configuration;
using JuniperBot.Model.Events;
using Ninject;

namespace JuniperBot.Services {

    internal class DiscordWebHookPoster : AbstractService {

        [Inject]
        public InstagramPoller InstagramPoller
        {
            get; set;
        }

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        public string LatestId
        {
            get;
            set;
        }

        private List<DiscordWebhookClient> WebHooks
        {
            get; set;
        } = new List<DiscordWebhookClient>();

        protected override void Init() {
            foreach (DiscordWebHookElement WebHook in ConfigurationManager.Config.Discord.WebHooks) {
                WebHooks.Add(new DiscordWebhookClient(WebHook.Id.Value, WebHook.Token));
            }
            InstagramPoller.Update += InstagramPoller_Update;
        }

        private bool _ResetToSecond = false;

        public void ResetToSecond() {
            lock (this) {
                _ResetToSecond = true;
            }
        }

        private async void InstagramPoller_Update(object sender, UpdateInstagramEventArgs e) {
            lock (this) {
                if (_ResetToSecond) {
                    if (e.Medias.Count > 1) {
                        LatestId = e.Medias[1].Id;
                    }
                    _ResetToSecond = false;
                }
            }

            if (LatestId != null) {
                List<Media> newMedias = new List<Media>();
                IEnumerator<Media> enumerator = e.Medias.GetEnumerator();
                while (enumerator.MoveNext() && !LatestId.Equals(enumerator.Current.Id)) {
                    newMedias.Add(enumerator.Current);
                }

                var size = Math.Min(PostCommand.MAX_DETAILED, newMedias.Count);
                if (size > 0) {
                    Embed[] embeds = new Embed[size];
                    for (int i = 0; i < size; i++) {
                        embeds[i] = PostCommand.ConvertToEmbed(null, newMedias[i]);
                    }
                    foreach (DiscordWebhookClient webHook in WebHooks) {
                        await webHook.SendMessageAsync("", false, embeds);
                    }
                }
            }
            LatestId = e.Medias[0].Id;
        }
    }
}