using System;
using JuniperBot.Model.Scheduler;
using Quartz;
using Quartz.Impl;

namespace JuniperBot.Services {

    internal class SchedulerService : AbstractService {
        private IScheduler scheduler;

        protected override void Init() {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            scheduler = schedFact.GetScheduler();
            scheduler.Start();
        }

        public void Schedule(AbstractJob job) {
            Type type = job.GetType();
            IJobDetail jobDetail = JobBuilder.Create(type)
                    .SetJobData(job.BuildJobDataMap())
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(job.DateTime)
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}