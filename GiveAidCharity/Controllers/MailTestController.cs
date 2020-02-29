using GiveAidCharity.Models.HelperClass;
using System.Web.Mvc;

namespace GiveAidCharity.Controllers
{
    public class MailTestController : Controller
    {
        // GET: MailTest
        public ActionResult Index()
        {
            var mailer = new Email
            {
                ToEmail = "anhdungpham090@gmail.com",
                Subject = "Verify your email id",
                Body =
                    "Thanks for Registering your account.<br> please verify your email id by clicking the link <br> <a href='https://giveaidcharity.azurewebsites.net/'>verify</a>",
                IsHtml = true
            };
            mailer.Send();
            return null;
        }
    }
}