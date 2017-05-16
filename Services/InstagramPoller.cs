using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using InstaSharp.Models;
using JuniperBot.Model.Events;
using Ninject;

namespace JuniperBot.Services {

    internal class InstagramPoller : AbstractService {

        public delegate void UpdateEvent(object sender, UpdateInstagramEventArgs e);

        private BackgroundWorker Worker;

        public event UpdateEvent Update;

        [Inject]
        public InstagramClient InstagramClient
        {
            get; set;
        }

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        private void OnUpdate(List<Media> Medias) {
            Update?.Invoke(this, new UpdateInstagramEventArgs(Medias));
        }

        protected override void Init() {
            Worker = new BackgroundWorker();
            Worker.DoWork += async (s, e) => {
                while (true) {
                    Thread.Sleep(ConfigurationManager.Config.SelfUpdateInterval);
                    List<Media> medias = await InstagramClient.GetRecent();
                    if (medias != null) {
                        if (medias.Count > 0) {
                            OnUpdate(medias);
                        }
                    }
                }
            };
            Worker.RunWorkerAsync();
        }
    }
}