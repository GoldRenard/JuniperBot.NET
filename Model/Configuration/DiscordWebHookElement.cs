using System.Configuration;

namespace JuniperBot.Model.Configuration {

    public class DiscordWebHookElement : ConfigurationElement {

        [ConfigurationProperty("Id", IsRequired = true)]
        public ulong? Id
        {
            get
            {
                return base["Id"] as ulong?;
            }
        }

        [ConfigurationProperty("Token", IsRequired = true)]
        public string Token
        {
            get
            {
                return base["Token"] as string;
            }
        }
    }
}