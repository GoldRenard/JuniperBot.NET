using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using JuniperBot.Commands;
using Ninject;

namespace JuniperBot.Services {

    internal class CommandManager : AbstractService {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(CommandManager));

        private ConcurrentDictionary<string, ICommand> Commands = new ConcurrentDictionary<string, ICommand>();

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        protected override void Init() {
            foreach (ICommand command in Program.Kernel.GetAll<ICommand>()) {
                RegisterCommand(command);
            }
        }

        public async Task<bool> Send(DiscordMessage message, string input) {
            if (string.IsNullOrEmpty(input)) {
                return false;
            }
            string[] args = input.Split(' ');
            if (!Commands.ContainsKey(args[0])) {
                return false;
            }
            ICommand command;
            Commands.TryGetValue(args[0], out command);
            if (command == null) {
                return false;
            }
            return await command.DoCommand(message, args);
        }

        public void RegisterCommand(ICommand command) {
            if (command == null) {
                throw new ArgumentException("command argument cannot be null");
            }
            if (Commands.ContainsKey(command.GetName())) {
                LOGGER.ErrorFormat("Can't register command {0} because command with this name already registered!", command.GetName());
                return;
            }
            Commands.TryAdd(command.GetName(), command);
        }

        public bool UnRegisterCommand(ICommand command) {
            if (command == null) {
                throw new ArgumentException("command argument cannot be null");
            }
            var configToRemove = Commands.FirstOrDefault(kvp => kvp.Value.Equals(command));
            if (configToRemove.Key != null) {
                return Commands.TryRemove(configToRemove.Key, out command);
            }
            return false;
        }

        public IDictionary<string, ICommand> GetCommands() {
            return new Dictionary<string, ICommand>(Commands);
        }
    }
}