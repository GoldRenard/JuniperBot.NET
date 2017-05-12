using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Commands;
using JuniperBot.Model;
using JuniperBot.Model.Exception;
using JuniperBot.Tools;
using Ninject;

namespace JuniperBot.Services {

    internal class CommandManager : AbstractService {
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(CommandManager));

        private ConcurrentDictionary<string, ICommand> Commands = new ConcurrentDictionary<string, ICommand>();

        private ConcurrentDictionary<ulong, BotContext> ChannelContexts = new ConcurrentDictionary<ulong, BotContext>();

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

        public async Task<bool> Send(SocketMessage message, string input) {
            if (string.IsNullOrEmpty(input)) {
                return false;
            }
            if (input.Length > 255) {
                return false;
            }
            string[] args = ReadArgs(input);
            if (args.Length == 0) {
                return false;
            }

            string commandKey = args[0];
            if (!Commands.ContainsKey(commandKey)) {
                return false;
            }

            ICommand command;
            Commands.TryGetValue(commandKey, out command);
            if (command == null) {
                return false;
            }

            BotContext context;
            ChannelContexts.TryGetValue(message.Channel.Id, out context);
            if (context == null) {
                context = new BotContext(message.Channel);
                ChannelContexts.TryAdd(message.Channel.Id, context);
            }

            try {
                return await command.DoCommand(message, context, args.SubArray(1));
            } catch (ValidationException e) {
                await message.Channel.SendMessageAsync(e.Message);
            } catch (Exception e) {
                await message.Channel.SendMessageAsync("Ой, произошла какая-то ошибка :C Покорми меня?");
                LOGGER.Error($"Command '{commandKey}' execution error!", e);
            }
            return true;
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

        /// <summary>
        /// Reads command line arguments from a single string.
        /// </summary>
        /// <param name="argsString">The string that contains the entire command line.</param>
        /// <returns>An array of the parsed arguments.</returns>
        public string[] ReadArgs(string argsString) {
            // Collects the split argument strings
            List<string> args = new List<string>();
            // Builds the current argument
            var currentArg = new StringBuilder();
            // Indicates whether the last character was a backslash escape character
            bool escape = false;
            // Indicates whether we're in a quoted range
            bool inQuote = false;
            // Indicates whether there were quotes in the current arguments
            bool hadQuote = false;
            // Remembers the previous character
            char prevCh = '\0';
            // Iterate all characters from the input string
            for (int i = 0; i < argsString.Length; i++) {
                char ch = argsString[i];
                if (ch == '\\' && !escape) {
                    // Beginning of a backslash-escape sequence
                    escape = true;
                } else if (ch == '\\' && escape) {
                    // Double backslash, keep one
                    currentArg.Append(ch);
                    escape = false;
                } else if (ch == '"' && !escape) {
                    // Toggle quoted range
                    inQuote = !inQuote;
                    hadQuote = true;
                    if (inQuote && prevCh == '"') {
                        // Doubled quote within a quoted range is like escaping
                        currentArg.Append(ch);
                    }
                } else if (ch == '"' && escape) {
                    // Backslash-escaped quote, keep it
                    currentArg.Append(ch);
                    escape = false;
                } else if (char.IsWhiteSpace(ch) && !inQuote) {
                    if (escape) {
                        // Add pending escape char
                        currentArg.Append('\\');
                        escape = false;
                    }
                    // Accept empty arguments only if they are quoted
                    if (currentArg.Length > 0 || hadQuote) {
                        args.Add(currentArg.ToString());
                    }
                    // Reset for next argument
                    currentArg.Clear();
                    hadQuote = false;
                } else {
                    if (escape) {
                        // Add pending escape char
                        currentArg.Append('\\');
                        escape = false;
                    }
                    // Copy character from input, no special meaning
                    currentArg.Append(ch);
                }
                prevCh = ch;
            }
            // Save last argument
            if (currentArg.Length > 0 || hadQuote) {
                args.Add(currentArg.ToString());
            }
            return args.ToArray();
        }
    }
}