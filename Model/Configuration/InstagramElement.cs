using System.Configuration;

namespace JuniperBot.Model.Configuration {

    public class InstagramElement : ConfigurationElement {

        [ConfigurationProperty("ClientId", IsRequired = true)]
        public string ClientId
        {
            get
            {
                return base["ClientId"] as string;
            }
        }

        [ConfigurationProperty("ClientSecret", IsRequired = true)]
        public string ClientSecret
        {
            get
            {
                return base["ClientSecret"] as string;
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

        [ConfigurationProperty("RedirectUri", IsRequired = true)]
        public string RedirectUri
        {
            get
            {
                return base["RedirectUri"] as string;
            }
        }

        [ConfigurationProperty("UserId", IsRequired = true)]
        public long UserId
        {
            get
            {
                return (long)base["UserId"];
            }
        }
    }
}