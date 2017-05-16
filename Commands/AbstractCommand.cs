using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;

namespace JuniperBot.Commands {

    /// <summary>
    /// Base <see cref="ICommand"/> implementation
    /// </summary>
    /// <seealso cref="CommandManager"/>
    /// <seealso cref="ICommand"/>
    /// <seealso cref="AbstractExtendedCommand"/>
    public abstract class AbstractCommand : ICommand {
        public readonly int MAX_MESSAGE_SIZE = 2000;

        private readonly string commandName;
        private readonly string commandDescription;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="commandName">Command name to execute. See <see cref="ICommand.GetName"/></param>
        /// <param name="commandDescription">Command description for help. See <see cref="ICommand.GetDescription"/></param>
        public AbstractCommand(string commandName, string commandDescription) {
            this.commandName = commandName;
            this.commandDescription = commandDescription;
        }

        /// <summary>
        /// The command action
        /// </summary>
        /// <param name="args">Input arguments</param>
        /// <returns><B>True</B> if command successfully executed, <B>false</B> otherwise.</returns>
        public abstract Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args);

        /// <summary>
        /// Command description
        /// </summary>
        /// <returns>Command description</returns>
        public virtual string GetDescription() {
            return commandDescription;
        }

        /// <summary>
        /// Command name to execute
        /// </summary>
        /// <returns>Command name</returns>
        public virtual string GetName() {
            return commandName;
        }

        /// <summary>
        /// Prints data in table view.
        /// </summary>
        /// <param name="ColumnNames">List of column headers</param>
        /// <param name="columns">List of columns data. This array should be the same size with ColumnNames param.</param>
        /// <returns></returns>
        protected List<string> PrintTable(List<string> ColumnNames, params List<string>[] columns) {
            int[] lenghts = new int[ColumnNames.Count];
            for (int i = 0; i < ColumnNames.Count; i++) {
                lenghts[i] = columns[i].Max(c => c.Length);
                if (lenghts[i] < ColumnNames[i].Length) {
                    lenghts[i] = ColumnNames[i].Length;
                }
                lenghts[i] = lenghts[i] + 2;
            }

            List<string> output = new List<string>();
            StringBuilder recordBuilder = new StringBuilder(" ");
            for (int columnName = 0; columnName < ColumnNames.Count; columnName++) {
                string value = ColumnNames[columnName];
                recordBuilder.Append(value);
                int spaceleft = lenghts[columnName] - value.Length;
                while (spaceleft > 0) {
                    recordBuilder.Append(" ");
                    spaceleft--;
                }
            }
            output.Add(recordBuilder.ToString());
            recordBuilder = new StringBuilder("");
            for (int i = 0; i < lenghts.Sum(); i++) {
                recordBuilder.Append("-");
            }
            output.Add(recordBuilder.ToString());

            for (int record = 0; record < columns[0].Count; record++) {
                recordBuilder = new StringBuilder(" ");
                for (int column = 0; column < columns.Length; column++) {
                    string value = columns[column][record];
                    recordBuilder.Append(value);
                    int spaceleft = lenghts[column] - value.Length;
                    while (spaceleft > 0) {
                        recordBuilder.Append(" ");
                        spaceleft--;
                    }
                }
                output.Add(recordBuilder.ToString());
            }
            return output;
        }

        public virtual bool Hidden
        {
            get
            {
                return false;
            }
        }
    }
}