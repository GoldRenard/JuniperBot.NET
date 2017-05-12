using Discord.WebSocket;

namespace JuniperBot.Model {

    public class BotContext {

        public bool DetailedEmbed
        {
            get;
            set;
        } = true;

        public string LatestId
        {
            get;
            set;
        }

        public readonly ISocketMessageChannel Channel;

        public BotContext(ISocketMessageChannel Channel) {
            this.Channel = Channel;
        }
    }
}