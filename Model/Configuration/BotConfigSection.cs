using System;
using System.Configuration;

namespace JuniperBot.Model.Configuration {

    public class BotConfigSection : ConfigurationSection {
        public const String CLIENT_ID = "310848622642069504";
        public const String CLIENT_SECRET = "UkcO5plrlpdo9PZ0qnK5hXniB8Iz5_PN";
        public const String BOT_TOKEN = "MzEwODQ4NjIyNjQyMDY5NTA0.C_D8hQ.C1oQgl5LBqvLJ5WNnujOQvUWIQM";

        public const string INSTAGRAM_CLIENT_ID = "85f82cc8b73d4db091c5ee1d98554f26";
        public const string INSTAGRAM_CLIENT_SECRET = "8914949920f14fde9363c7caf60dad55";
        public const string INSTAGRAM_REDIRECT_URI = "http://193.124.56.233";
        public const string INSTAGRAM_TOKEN = "5430448400.ba4c844.b0d64b7cc06944839bc45c8af6f074c5";

        [ConfigurationProperty("Discord", IsRequired = true)]
        public DiscordElement Discord
        {
            get
            {
                return (DiscordElement)(base["Discord"]);
            }
        }

        [ConfigurationProperty("Instagram", IsRequired = true)]
        public InstagramElement Instagram
        {
            get
            {
                return (InstagramElement)(base["Instagram"]);
            }
        }
    }
}