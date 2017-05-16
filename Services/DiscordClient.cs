using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
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

        [Inject]
        public DiscordWebHookPoster DiscordWebHookPoster
        {
            get; set;
        }

        public DiscordSocketClient Client
        {
            get;
            private set;
        }

        private DiscordSocketConfig Config;

        private bool Connected
        {
            get;
            set;
        }

        protected override void Init() {
            Config = new DiscordSocketConfig() {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 100
            };
            Client = new DiscordSocketClient(Config);
            Client.MessageReceived += OnMessageReceived;
            Client.Log += OnLogMessageReceived;
        }

        public async Task Connect() {
            await Client.LoginAsync(TokenType.Bot, ConfigurationManager.Config.Discord.Token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task Disconnect() {
            if (Client.ConnectionState == ConnectionState.Connected || Client.ConnectionState == ConnectionState.Disconnected) {
                await Client.StopAsync();
                await Client.LogoutAsync();
                Client.Dispose();
            }
        }

        private async Task OnMessageReceived(SocketMessage e) {
            if (e.Author.IsBot) {
                return;
            }

            string input = e.Content;
            if (!string.IsNullOrEmpty(input)) {
                if (input.StartsWith(ConfigurationManager.Config.Discord.CommandPrefix)) {
                    input = input.Substring(ConfigurationManager.Config.Discord.CommandPrefix.Length);
                    await CommandManager.Send(e, input);
                }
            }
        }

        private async Task OnLogMessageReceived(LogMessage logMessage) {
            Action<string> log = LOGGER.Info;
            Action<string, Exception> logEx = LOGGER.Info;

            switch (logMessage.Severity) {
                case LogSeverity.Critical:
                    log = LOGGER.Fatal;
                    logEx = LOGGER.Fatal;
                    break;

                case LogSeverity.Debug:
                    log = LOGGER.Debug;
                    logEx = LOGGER.Debug;
                    break;

                case LogSeverity.Error:
                    log = LOGGER.Error;
                    logEx = LOGGER.Error;
                    break;

                case LogSeverity.Warning:
                    log = LOGGER.Warn;
                    logEx = LOGGER.Warn;
                    break;

                default:
                    log = LOGGER.Info;
                    logEx = LOGGER.Info;
                    break;
            }
            if (logMessage.Exception != null) {
                logEx(logMessage.Message, logMessage.Exception);
            } else {
                log(logMessage.Message);
            }
            await Task.Delay(0);
        }
    }
}