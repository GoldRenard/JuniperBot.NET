using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord.WebSocket;
using JuniperBot.Model;
using JuniperBot.Model.Scheduler;
using JuniperBot.Services;
using Ninject;

namespace JuniperBot.Commands.Phyr {

    internal class RemindCommand : AbstractCommand {

        private const string DATE_FORMAT = "dd.MM.yyyy HH:mm";

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

        private readonly TimeZoneInfo timeZoneInfo;

        public RemindCommand()
            : base("напомни", "Напомнить о чем-либо. Дата в формате дд.ММ.гггг чч:мм и сообщение в кавычках") {
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        }

        public async override Task<bool> DoCommand(SocketMessage message, BotContext context, string[] args) {
            if (args.Length < 2) {
                return await PrintHelp(message);
            }

            try {
                DateTimeOffset dateOffset = ParseDateExactForTimeZone(args[0], timeZoneInfo);
                if (dateOffset.ToUniversalTime() < DateTime.UtcNow) {
                    await message.Channel.SendMessageAsync("Указывай дату в будущем, пожалуйста");
                    return false;
                }
                SchedulerService.Schedule(new MessageJob(dateOffset, context, args[1]));
                await message.Channel.SendMessageAsync("Лаааадно, напомню. Фыр.");
            } catch (Exception e) {
                return await PrintHelp(message);
            }
            return true;
        }

        public DateTimeOffset ParseDateExactForTimeZone(string dateTime, TimeZoneInfo timezone) {
            var parsedDateLocal = DateTimeOffset.ParseExact(dateTime, DATE_FORMAT, CultureInfo.InvariantCulture);
            var tzOffset = timezone.GetUtcOffset(parsedDateLocal.DateTime);
            var parsedDateTimeZone = new DateTimeOffset(parsedDateLocal.DateTime, tzOffset);
            return parsedDateTimeZone;
        }

        private async Task<bool> PrintHelp(SocketMessage message) {
            await message.Channel.SendMessageAsync(
                string.Format(@"Дата в формате дд.ММ.гггг чч:мм и сообщение в кавычках. Например: {0}напомни ""03.07.2017 21:27"" ""Сообщение""",
                ConfigurationManager.Config.Discord.CommandPrefix));
            return false;
        }
    }
}