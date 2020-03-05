using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GiveAidCharity.Models.Main;
using Quartz;

namespace GiveAidCharity.Models.HelperClass
{
    public class FollowEmailJob : IJob
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var followList = await _db.Follows.Where(f =>
                    f.Status == Follow.FollowStatusEnum.Followed &&
                    f.Project.Status == Project.ProjectStatusEnum.Ongoing).ToListAsync();
                foreach (var project in followList.Select(item => item.Project))
                {
                    if (project == null) continue;
                    var body = HelperMethod.ProjectPrepareTemplate(project);

                    var mailer = new Email
                    {
                        ToEmail = "anhdungpham090@gmail.com",
                        Subject = "Keep Track On Our Cause",
                        Body = body,
                        IsHtml = true
                    };
                    mailer.Send();

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class RemovePendingTransactionJob : IJob
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var donations = await _db.Donations
                    .Where(d => d.CreatedAt <= HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow).AddMinutes(-5) && 
                                d.Status == Donation.DonationStatusEnum.Pending)
                    .ToListAsync();
                foreach (var item in donations)
                {
                    item.Status = Donation.DonationStatusEnum.Cancel;
                    _db.Entry(item).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    HelperMethod.NotifyEmailTransaction("Transaction no " + item.Id + " is" +
                                                        item.Status, "Transaction Result", 
                                                        item, "Transaction result", 
                                                        "anhdungpham090@gmail.com");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class NotifyProjectProgressJob : IJob
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var projects = await _db.Projects.Where(p => p.Status == Project.ProjectStatusEnum.Ongoing)
                    .ToListAsync();
                foreach (var mailer in from project in projects where project != null 
                    let body = HelperMethod.ProjectPrepareTemplate(project) 
                    select new Email {
                        ToEmail = project.ApplicationUser.Email,
                        Subject = "Keep Track On your Cause",
                        Body = body,
                        IsHtml = true
                    })
                {
                    mailer.Send();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class ExpireProjectJob : IJob
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var projects = await _db.Projects.Where(p => p.Status == Project.ProjectStatusEnum.Ongoing && 
                                                             p.ExpireDate <= HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow))
                    .ToListAsync();
                foreach (var item in projects)
                {
                    if(item.CurrentFund < item.Goal)
                    {
                        item.Status = Project.ProjectStatusEnum.Suspended;
                        _db.Entry(item).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                        await HelperMethod.FailProject(item.Id);
                    }
                    else
                    {
                        item.Status = Project.ProjectStatusEnum.Success;
                        _db.Entry(item).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                        await HelperMethod.SuccessProject(item.Id);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }
    }
}