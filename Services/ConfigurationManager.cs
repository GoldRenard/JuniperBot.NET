using System;
using System.Configuration;
using JuniperBot.Model.Configuration;

namespace JuniperBot.Services {

    internal class ConfigurationManager : AbstractService {
        private Configuration ExeConfiguration;

        public BotConfigSection Config
        {
            get;
            private set;
        }

        protected override void Init() {
            ExeConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Config = ExeConfiguration.GetSection("BotConfigSection") as BotConfigSection;
            if (Config == null) {
                throw new Exception("No configuration found");
            }
        }

        public void Save() {
            ExeConfiguration.Save();
        }
    }
}