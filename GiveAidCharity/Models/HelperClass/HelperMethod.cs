using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using GiveAidCharity.Models.Main;

namespace GiveAidCharity.Models.HelperClass
{
    public class HelperMethod
    {
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();

        public static bool CheckValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }

        public static string SubString(string text)
        {
            if (text.Length > 5)
            {
                return text.Substring(5) + "...";
            }

            return text;
        }

        internal static async Task<int> RatingCount(string id)
        {
            if (id == null) return -(int) HttpStatusCode.BadRequest;
            var blog = await Db.Blogs.FindAsync(id);
            if (blog == null) return -(int)HttpStatusCode.NotFound;
            var votes = await Db.Votes.Where(d => d.Status != Vote.VoteStatusEnum.Neutral && d.BlogId == id).ToListAsync();
            return votes.Sum(item => (int) item.Status);
        }
        public static string ProjectPrepareTemplate(Project project)
        {
            string body;
            try
            {
                using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/emailtemp.html")))
                {
                    body = reader.ReadToEnd();
                    //Replace variables available in body Stream
                    body = body.Replace("{Title}", "Keep Track On Our Cause");
                    body = body.Replace("{Content1}", project.Name);
                    body = body.Replace("{Url}", 
                        "http://giveaidcharity.azurewebsites.net/Home/CauseDetail/" + project.Id);
                    body = body.Replace("{Content2}", project.Status.ToString());
                    body = body.Replace("{Content3}",
                        project.CurrentFund >= project.Goal ? 100.ToString() :
                            (project.CurrentFund * 100 / project.Goal).ToString(CultureInfo.InvariantCulture) + "%");
                    body = body.Replace("{Content4}", "Current Fund: " +
                        "$" + project.CurrentFund.ToString(CultureInfo.InvariantCulture));
                    body = body.Replace("{Content5}", "Goal: " +
                        "$" + project.Goal.ToString(CultureInfo.InvariantCulture));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
            return body;
        }

        public static DateTime GetCurrentDateTimeWithTimeZone(DateTime dateIn) // dateIn must be UTC, DateTime.UtcNow
        {
            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateIn,
                    TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }

        public static string ProjectSuccessPrepareTemplate(string message, string title, Project project)
        {
            string body;
            try
            {
                using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/emailtemp.html")))
                {
                    body = reader.ReadToEnd();
                    //Replace variables available in body Stream
                    body = body.Replace("{Title}", title);
                    body = body.Replace("{Content1}", message);
                    body = body.Replace("{Url}",
                        "http://giveaidcharity.azurewebsites.net/Payment/Result/" + project.Id);
                    body = body.Replace("{Content2}", "Name: " + project.Name);
                    body = body.Replace("{Content3}", "Fund raised: " + project.CurrentFund);
                    body = body.Replace("{Content4}", "Goal: " + project.Goal);
                    body = body.Replace("{Content5}", "");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
            return body;
        }
        public static string TransactionPrepareTemplate(string message, string title, Donation donation)
        {
            string body;
            try
            {
                using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/emailtemp.html")))
                {
                    body = reader.ReadToEnd();
                    //Replace variables available in body Stream
                    body = body.Replace("{Title}", title);
                    body = body.Replace("{Content1}", message);
                    body = body.Replace("{Url}",
                        "http://giveaidcharity.azurewebsites.net/Payment/Result/" + donation.Id);
                    body = body.Replace("{Content2}", "Date: " + donation.CreatedAt);
                    body = body.Replace("{Content3}", "Payment Method: " + donation.PaymentMethod);
                    body = body.Replace("{Content4}", "Status: " + donation.Status);
                    body = body.Replace("{Content5}", "Amount: " + donation.Amount);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
            return body;
        }
        public static void NotifyEmailProjectSuccess(string message, string title, Project project, string subject, string email)
        {
            try
            {
                var body = ProjectSuccessPrepareTemplate(message, title, project);

                var mailer = new Email
                {
                    ToEmail = email,
                    Subject = subject,
                    Body = body,
                    IsHtml = true
                };
                mailer.Send();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }
        public static void NotifyEmailTransaction(string message, string title, Donation donation, string subject, string email)
        {
            try
            {
                var body = TransactionPrepareTemplate(message, title, donation);

                var mailer = new Email
                {
                    ToEmail = email,
                    Subject = subject,
                    Body = body,
                    IsHtml = true
                };
                mailer.Send();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task FailProject(string id)
        {
            if (id == null) return;
            var project = await Db.Projects.FindAsync(id);
            if(project == null) return;
            foreach (var item in project.Donations.Where(d => d.Status == Donation.DonationStatusEnum.Success).ToList())
            {
                Refund(item, "Refund donation due to the project you donated to fail to success");
            }
        }

        public static void Refund(Donation donation , string message)
        {
            NotifyEmailTransaction(message,"Refund", donation, "Refund", "anhdungpham090@gmail.com");
        }

        public static async Task SuccessProject(string id)
        {
            if (id == null) return;
            var project = await Db.Projects.FindAsync(id);
            if (project == null) return;
            NotifyEmailProjectSuccess("The project " + project.Name + " has been success", 
                "Project success", project, "Project success", "anhdungpham090@gmail.com");
        }
    }
}