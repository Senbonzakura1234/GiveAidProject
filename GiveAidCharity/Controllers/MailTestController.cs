using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using GiveAidCharity.Models.HelperClass;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.Main;

namespace GiveAidCharity.Controllers
{
    public class MailTestController : Controller
    {
        // ReSharper disable  StringLiteralTypo
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private string ReadTemplate(Project project)
        {
            string body;
            using (var reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Content/emailtemp.html")))
            {
                body = reader.ReadToEnd();
                //Replace UserName and Other variables available in body Stream
                body = body.Replace("{ProjectName}", project.Name);
                if (Request.Url != null) body = body.Replace("{ProjectUrl}", 
                    Url.Action("CauseDetail", "Home", 
                    new{ id = project.Id}, Request.Url.Scheme));
                body = body.Replace("{ProjectStatus}", project.Status.ToString());
                body = body.Replace("{ProjectProgress}", 
                    project.CurrentFund >= project.Goal? 100.ToString() : 
                        (project.CurrentFund*100/project.Goal).ToString(CultureInfo.InvariantCulture) + "%");
                body = body.Replace("{CurrentFund}", 
                    "$" + project.CurrentFund.ToString(CultureInfo.InvariantCulture));
                body = body.Replace("{Goal}", 
                    "$" + project.Goal.ToString(CultureInfo.InvariantCulture));
            }
            return body;
        }
        // GET: MailTest
        public async Task<ActionResult> Index()
        {
            var project = await _db.Projects.FirstAsync();
            if (project == null) return HttpNotFound();
            var body = ReadTemplate(project);

            var mailer = new Email
            {
                ToEmail = "anhdungpham090@gmail.com",
                Subject = "Keep Track On Our Cause",
                Body = body,
                IsHtml = true
            };
            mailer.Send();
            return null;
        }
    }
}




