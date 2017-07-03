using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;
using JuniperBot.Model.Scheduler;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands.Phyr {

    internal class RemindCommand : AbstractCommand {
        private const string DATE_FORMAT = "dd.MM.yyyy HH:mmzzz";

        [Inject]
        public SchedulerService SchedulerService
        {
            get; set;
        }

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        public RemindCommand()
            : base("напомни", "Напомнить о чем-либо. Дата в формате дд.ММ.гггг чч:мм и сообщение в кавычках") {
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            if (args.Length < 2) {
                return await PrintHelp(message);
            }

            try {
                DateTime date;
                DateTime.TryParse(args[0] + "+04:00", out date);

                if (date < DateTime.Now) {
                    await message.Channel.SendMessageAsync("Указывай дату в будущем, пожалуйста");
                    return false;
                }
                SchedulerService.Schedule(new MessageJob(date, context, args[1]));
                await message.Channel.SendMessageAsync("Лаааадно, напомню. Фыр.");
            } catch (Exception e) {
                return await PrintHelp(message);
            }
            return true;
        }

        private async Task<bool> PrintHelp(SocketMessage message) {
            await message.Channel.SendMessageAsync(
                string.Format(@"Дата в формате дд.ММ.гггг чч:мм:сс и сообщение в кавычках. Например: {0}напомни ""03.07.2017 21:27"" ""Сообщение""",
                ConfigurationManager.Config.Discord.CommandPrefix));
            return false;
        }
    }
}