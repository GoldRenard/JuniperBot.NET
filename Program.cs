using System;
using JuniperBot.Modules;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot {

    internal class Program {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(Program));

        public static IKernel Kernel
        {
            get;
            private set;
        }

        private static void Main(string[] args) {
            Console.Title = "Juniper Bot";
            log4net.Config.XmlConfigurator.Configure();
            Kernel = new StandardKernel(new DependencyModule());
            AppDomain.CurrentDomain.UnhandledException += (s, e) => {
                LOGGER.Error("UnhandledException", e.ExceptionObject as Exception);
            };
            DiscordClient Client = Kernel.Get<DiscordClient>();
            Client.Connect().GetAwaiter().GetResult();
        }
    }
}