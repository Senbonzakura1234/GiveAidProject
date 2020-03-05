using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    [Authorize(Roles = "1Administrator, 2Moderator, 3FundRaiser")]
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
        [AllowAnonymous]
        public ActionResult DonationList()
        {
            var data = _db.Donations
                .Where(d => d.Status == Donation.DonationStatusEnum.Success)
                .OrderByDescending(d => d.CreatedAt).Take(10).ToList();
            var listDonation = data.OrderByDescending(d => d.Amount).Take(6).Select(s => new DonationListViewModel
            {
                Id = s.Id,
                Amount = s.Amount,
                UserName = s.ApplicationUser.UserName,
                UserId = s.ApplicationUserId,
                Avatar = s.ApplicationUser.Avatar,
                ProjectId = s.ProjectId,
                ProjectName = s.Project.Name
            }).ToList();

            return PartialView(listDonation);
        }
        [AllowAnonymous]
        public ActionResult ProjectList()
        {
            var data = _db.Projects.Where(d => d.Status == Project.ProjectStatusEnum.Ongoing)
                .OrderByDescending(d => d.CreatedAt).Take(5).Select(s => new ProjectListViewModel
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