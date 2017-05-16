using System.Configuration;

namespace JuniperBot.Model.Configuration {

    [ConfigurationCollection(typeof(DiscordWebHookElement), AddItemName = "WebHook")]
    public class DiscordWebHookCollection : ConfigurationElementCollection {

        protected override ConfigurationElement CreateNewElement() {
            return new DiscordWebHookElement();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((DiscordWebHookElement)(element)).Token;
        }

        public DiscordWebHookElement this[int idx]
        {
            get
            {
                return (DiscordWebHookElement)BaseGet(idx);
            }
        }
    }
}