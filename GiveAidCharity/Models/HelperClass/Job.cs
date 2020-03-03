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
                    .Where(d => d.CreatedAt <= DateTime.Now.AddMinutes(-5) && d.Status == Donation.DonationStatusEnum.Pending)
                    .ToListAsync();
                foreach (var item in donations)
                {
                    item.Status = Donation.DonationStatusEnum.Cancel;
                    _db.Entry(item).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    HelperMethod.NotifyEmailTransaction("Transaction no " + item.Id + " is" +
                                                        item.Status, "Transaction Result", item);
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