using System;
using System.Collections.Generic;
using InstaSharp.Models;

namespace JuniperBot.Model.Events {

    internal class UpdateInstagramEventArgs : EventArgs {

        public List<Media> Medias
        {
            get;
            private set;
        }

        public UpdateInstagramEventArgs(List<Media> Medias) {
            this.Medias = Medias;
        }
    }
}