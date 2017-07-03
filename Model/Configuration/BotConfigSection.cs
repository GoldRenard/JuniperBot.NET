using System.Configuration;

namespace JuniperBot.Model.Configuration {

    public class BotConfigSection : ConfigurationSection {

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

        [ConfigurationProperty("SelfUpdateInterval", IsRequired = true)]
        public int SelfUpdateInterval
        {
            get
            {
                return (int)(base["SelfUpdateInterval"]);
            }
        }
    }
}