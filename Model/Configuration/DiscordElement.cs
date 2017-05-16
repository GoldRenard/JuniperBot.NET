using System.Configuration;

namespace JuniperBot.Model.Configuration {

    public class DiscordElement : ConfigurationElement {

        [ConfigurationProperty("Token", IsRequired = true)]
        public string Token
        {
            get
            {
                return base["Token"] as string;
            }
        }

        [ConfigurationProperty("CommandPrefix", IsRequired = true)]
        public string CommandPrefix
        {
            get
            {
                return base["CommandPrefix"] as string;
            }
        }

        [ConfigurationProperty("WebHooks")]
        public DiscordWebHookCollection WebHooks
        {
            get
            {
                return ((DiscordWebHookCollection)(base["WebHooks"]));
            }
        }
    }
}