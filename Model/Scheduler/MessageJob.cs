using System;

namespace JuniperBot.Model.Scheduler {

    internal class MessageJob : AbstractJob {

        public string Message
        {
            set;
            get;
        }

        public MessageJob() {
            // default constructor
        }

        public MessageJob(DateTime DateTime, BotContext botContext, string message) : base(DateTime, botContext) {
            this.Message = message;
        }

        protected override void Execute(BotContext botContext) {
            botContext.Channel.SendMessageAsync(Message);
        }
    }
}