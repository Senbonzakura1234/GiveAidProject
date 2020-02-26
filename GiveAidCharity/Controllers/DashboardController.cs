using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public DashboardController()
        {
        }

        public DashboardController(
            ApplicationUserManager userManager
        )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DonationList()
        {
            var data = _db.Donations.OrderByDescending(d => d.CreatedAt).Take(10).ToList();
            var listDonation = data.OrderByDescending(d => d.Amount).Take(6).Select(s => new DonationListViewModel
            {
                Id = s.Id,
                Status = s.Status,
                UserName = s.ApplicationUser.UserName,
                UserId = s.ApplicationUserId,
                Avatar = s.ApplicationUser.Avatar,
                ProjectId = s.ProjectId,
                ProjectName = s.Project.Name
            }).ToList();

            return PartialView(listDonation);
        }

        public ActionResult ProjectList()
        {
            var data = _db.Projects.OrderByDescending(d => d.CreatedAt).Take(5).Select(s => new ProjectListViewModel
            {
                CreatedAt = s.CreatedAt,
                Status = s.Status,
                Name = s.Name,
                CurrentFund = s.CurrentFund,
                Id = s.Id,
                Goal = s.Goal,
                CoverImg = s.CoverImg,
                ExpireDate = s.ExpireDate,
                StartDate = s.StartDate
            }).ToList();
          
            return PartialView(data);
        }
    }
}