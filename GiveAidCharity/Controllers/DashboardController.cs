using System.Web.Mvc;

namespace GiveAidCharity.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}