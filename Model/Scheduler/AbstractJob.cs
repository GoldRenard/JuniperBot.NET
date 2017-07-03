using System;
using System.Linq;
using System.Reflection;
using Quartz;

namespace JuniperBot.Model.Scheduler {

    internal abstract class AbstractJob : IJob {
        public readonly DateTimeOffset DateTimeOffset;

        public BotContext BotContext
        {
            set;
            get;
        }

        protected AbstractJob() {
            // default constructor
        }

        public AbstractJob(DateTimeOffset DateTimeOffset, BotContext context) {
            this.DateTimeOffset = DateTimeOffset;
            this.BotContext = context;
        }

        public void Execute(IJobExecutionContext context) {
            ReadFromJobDataMap(context.MergedJobDataMap);
            Execute(context.MergedJobDataMap["BotContext"] as BotContext);
        }

        protected abstract void Execute(BotContext botContext);

        #region JobDataMap & Serialization

        public JobDataMap BuildJobDataMap() {
            JobDataMap data = new JobDataMap();
            foreach (var prop in GetType().GetProperties()) {
                object value = prop.GetValue(this, null);
                data.Add(prop.Name, prop.GetValue(this, null));
            }
            return data;
        }

        private void ReadFromJobDataMap(JobDataMap data) {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (var key in data.Keys) {
                var p = properties.Where(x => x.Name == key).SingleOrDefault();
                if (p != null) {
                    p.SetValue(this, data.Get(key), null);
                }
            }
        }

        #endregion JobDataMap & Serialization
    }
}