using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public HomeController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        public HomeController(
            ApplicationUserManager userManager,
            RoleManager<ApplicationUserRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public RoleManager<ApplicationUserRole> RoleManager
        {
            get => _roleManager ?? new RoleManager<ApplicationUserRole>(new RoleStore<ApplicationUserRole>(_db));
            private set => _roleManager = value;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Causes(int? page, int? limit)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 9;
            }
            //.Where(p => p.Status == Project.ProjectStatusEnum.Ongoing)
            var projectsList = await _db.Projects.ToListAsync();
            var causesList = projectsList.Select(t => new CausesListViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    CurrentFund = t.CurrentFund,
                    Goal = t.Goal,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    ExpireDate = t.ExpireDate,
                    FollowCount = t.Follows.Count
                })
                .ToList();

            ViewBag.TotalPage = Math.Ceiling((double)causesList.Count / limit.Value);
            ViewBag.CurrentPage = page;

            ViewBag.Limit = limit;

            ViewBag.TotalItem = causesList.Count;

            causesList = causesList.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

            foreach (var variable in causesList)
            {
                Debug.WriteLine("Cause name: " + variable.Name);
            }
            return View(causesList);
        }

        public async Task<ActionResult> CauseDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var images = await _db.ProjectImages.Where(p => p.ProjectId == id).ToListAsync();
            var comments = await _db.ProjectComments.Where(p => p.ProjectId == id).ToListAsync();

            var cause = new CausesDetailViewModel
            {
                Name = project.Name,
                Status = project.Status,
                StartDate = project.StartDate,
                ExpireDate = project.ExpireDate,
                Goal = project.Goal,
                CurrentFund = project.CurrentFund,
                Description = project.Description,
                FollowCount = project.Follows.Count,
                ProjectImages = images,
                ProjectComments = comments
            };
            return View();
        }

        public ActionResult Donations()
        {
            return View();
        }
    }
}