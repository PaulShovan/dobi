using Dhobi.DependencyInjection;
using Dhobi.Service.Implementation;
using Dhobi.Service.Interface;
using Ninject;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Service.Notification
{
    public class NotificationScheduler
    {
        public void Start()
        {
            //System.Diagnostics.Debugger.Launch();
            var notificationService = DependencyResolver.Kernel.Get<NotificationService>();
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SendNotification>().Build();
            job.JobDataMap["service"] = notificationService;
            ITrigger trigger = TriggerBuilder.Create()
                   .WithSimpleSchedule(a => a.WithIntervalInSeconds(15).RepeatForever())
                   .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public void Stop()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown();
        }
    }
}
