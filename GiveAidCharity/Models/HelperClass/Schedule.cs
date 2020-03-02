using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace GiveAidCharity.Models.HelperClass
{
    public class Schedule
    {
        public static async void Start()

        {

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await scheduler.Start();



            var job = JobBuilder.Create<Job>().Build();



            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")

                .StartNow()

                .WithSimpleSchedule(x => x

                    .WithIntervalInSeconds(10)

                    .RepeatForever())

                .Build();



            await scheduler.ScheduleJob(job, trigger);

        }
    }
}