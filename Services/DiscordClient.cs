using System;
using System.Threading.Tasks;
using DSharpPlus;
using Ninject;

namespace JuniperBot.Services {

    internal class DiscordClient : AbstractService {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(DiscordClient));

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        [Inject]
        public CommandManager CommandManager
        {
            get; set;
        }

        private DSharpPlus.DiscordClient Client;

        private DiscordConfig Config;

        protected override void Init() {
            Config = new DiscordConfig {
                AutoReconnect = true,
                DiscordBranch = Branch.Stable,
                LargeThreshold = 250,
                LogLevel = LogLevel.Unnecessary,
                Token = ConfigurationManager.Config.Discord.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = false
            };
            Client = new DSharpPlus.DiscordClient(Config);
            Client.DebugLogger.LogMessageReceived += OnLogMessageReceived;
            Client.MessageCreated += OnMessageReceived;
            Client.GuildAvailable += e => {
                Client.DebugLogger.LogMessage(LogLevel.Info, "discord bot", $"Guild available: {e.Guild.Name}", DateTime.Now);
                return Task.Delay(0);
            };
        }

        public async Task Connect() {
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private async Task OnMessageReceived(MessageCreateEventArgs e) {
            string input = e.Message.Content;
            if (!string.IsNullOrEmpty(input)) {
                if (input.StartsWith(ConfigurationManager.Config.Discord.CommandPrefix)) {
                    input = input.Substring(ConfigurationManager.Config.Discord.CommandPrefix.Length);
                    await CommandManager.Send(e.Message, input);
                }
            }
        }

        private void OnLogMessageReceived(object sender, DebugLogMessageEventArgs e) {
            Action<string> log = LOGGER.Info;

            switch (e.Level) {
                case LogLevel.Critical:
                    log = LOGGER.Fatal;
                    break;

                case LogLevel.Debug:
                    log = LOGGER.Debug;
                    break;

                case LogLevel.Error:
                    log = LOGGER.Error;
                    break;

                case LogLevel.Warning:
                    log = LOGGER.Warn;
                    break;

                default:
                    log = LOGGER.Info;
                    break;
            }
            log(string.Format("{0}: {1}", e.Application, e.Message));
        }
    }
}