using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace GiveAidCharity.Models.HelperClass
{
    public class Job : IJob
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public async Task Execute(IJobExecutionContext context)
        {
            var project = await _db.Projects.FindAsync("02b751ab-548c-4695-8f3a-44e87be832ef");
            if (project == null)
            {
                Debug.WriteLine("project not found");
                return;
            }
            project.Goal += 1;
            _db.Entry(project).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            Debug.WriteLine("project goal increased");
        }
    }
}