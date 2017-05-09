using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;

namespace JuniperBot.Commands {

    /// <summary>
    /// Command interface
    /// </summary>
    /// <seealso cref="CommandManager"/>
    /// <seealso cref="AbstractCommand"/>
    /// <seealso cref="AbstractExtendedCommand"/>
    public interface ICommand {

        /// <summary>
        /// The command action
        /// </summary>
        /// <param name="args">Input arguments</param>
        /// <returns><B>True</B> if command successfully executed, <B>False</B> otherwise.</returns>
        Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args);

        /// <summary>
        /// Command description
        /// </summary>
        /// <returns>Command description</returns>
        string GetDescription();

        /// <summary>
        /// Command name to execute
        /// </summary>
        /// <returns>Command name</returns>
        string GetName();
    }
}