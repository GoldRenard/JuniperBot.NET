using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using InstaSharp.Models;
using JuniperBot.Model;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands.Phyr {

    internal class AutoPostCommand : PostCommand {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(AutoPostCommand));

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        private List<BotContext> Contexts = new List<BotContext>();

        private BackgroundWorker Worker;

        public AutoPostCommand()
            : base("нафыркивай", "Автоматически нафыркивать новые посты из блога Джупи") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            bool added = false;
            lock (Contexts) {
                if (!Contexts.Contains(context)) {
                    added = true;
                    Contexts.Add(context);
                }
            }
            if (!added) {
                await context.Channel.SendMessageAsync("Ты меня уже просил нафыркивать!");
                return true;
            } else {
                await context.Channel.SendMessageAsync("Хорошо! Как только будет что-то новенькое я сюда фыркну ^_^");
            }

            lock (this) {
                if (Worker == null) {
                    Worker = new BackgroundWorker();
                    Worker.DoWork += UpdateInterval;
                    Worker.RunWorkerAsync();
                }
            }
            return true;
        }

        private async void UpdateInterval(object sender, DoWorkEventArgs e) {
            while (true) {
                try {
                    List<Media> medias = await InstagramClient.GetRecent();
                    if (medias.Count > 0) {
                        foreach (BotContext context in Contexts) {
                            if (context.LatestId != null) {
                                List<Media> newMedias = new List<Media>();
                                IEnumerator<Media> enumerator = medias.GetEnumerator();
                                while (enumerator.MoveNext() && !context.LatestId.Equals(enumerator.Current.Id)) {
                                    newMedias.Add(enumerator.Current);
                                }

                                if (newMedias.Count > 0) {
                                    await Post(newMedias, context);
                                }
                            }
                            context.LatestId = medias[0].Id;
                        }
                    }
                } catch (Exception ex) {
                    LOGGER.Error("Autopost error", ex);
                }
                Thread.Sleep(ConfigurationManager.Config.SelfUpdateInterval);
            }
        }
    }
}