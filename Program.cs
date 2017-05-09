using System;
using System.Runtime.InteropServices;
using JuniperBot.Modules;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot {

    internal class Program {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(Program));

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);

        private static EventHandler CloseHandler;

        private enum CtrlType {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static IKernel Kernel
        {
            get;
            private set;
        }

        private static DiscordClient Client
        {
            get;
            set;
        }

        private static void Main(string[] args) {
            Console.Title = "Juniper Bot";
            log4net.Config.XmlConfigurator.Configure();
            Kernel = new StandardKernel(new DependencyModule());
            AppDomain.CurrentDomain.UnhandledException += (s, e) => {
                LOGGER.Error("UnhandledException", e.ExceptionObject as Exception);
            };
            CloseHandler += new EventHandler(OnClose);
            SetConsoleCtrlHandler(CloseHandler, true);

            Client = Kernel.Get<DiscordClient>();
            Client.Connect().GetAwaiter().GetResult();
        }

        private static bool OnClose(CtrlType sig) {
            switch (sig) {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                default:
                    Client.Disconnect().GetAwaiter().GetResult();
                    return false;
            }
        }
    }
}