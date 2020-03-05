using System;
using System.Diagnostics;
using Quartz;
using Quartz.Impl;

namespace GiveAidCharity.Models.HelperClass
{
    internal class FollowEmailSchedule
    {
        public static async void Start()
        {
            try
            {
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();
                var job = JobBuilder.Create<FollowEmailJob>().Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .WithSchedule(CronScheduleBuilder
                        .WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 8, 45)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")))
                    .Build();
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
               
            }
        }
    }

    internal class RemovePendingTransactionSchedule
    {
        public static async void Start()
        {
            try
            {
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();
                var job = JobBuilder.Create<RemovePendingTransactionJob>().Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger2", "group2").StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(10)
                        .RepeatForever())
                    .Build();
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
               
            }
        }
    }

    internal class NotifyProjectProgressSchedule
    {
        public static async void Start()
        {
            try
            {
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();
                var job = JobBuilder.Create<NotifyProjectProgressJob>().Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger3", "group3")
                    .WithSchedule(CronScheduleBuilder
                        .WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 8, 45)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")))
                    .Build();
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
               
            }
        }
    }

    internal class ExpireProjectSchedule
    {
        public static async void Start()
        {
            try
            {
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();
                var job = JobBuilder.Create<ExpireProjectJob>().Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger4", "group4")
                    .WithSchedule(CronScheduleBuilder
                        .DailyAtHourAndMinute( 23, 59)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")))
                    .Build();
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
               
            }
        }
    }
}