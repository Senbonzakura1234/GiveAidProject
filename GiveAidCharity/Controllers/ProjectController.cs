using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public ProjectController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        public ProjectController(
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
        // GET: Project
        public ActionResult Index()
        {
            var projects = _db.Projects.ToList();
            var projectList = new List<ProjectListViewModel>();
            foreach(var item in projects)
            {
                projectList.Add(new ProjectListViewModel { 
                    Id = item.Id,
                    Name = item.Name,
                    CoverImg = item.CoverImg,
                    Goal = item.Goal,
                    CurrentFund = item.CurrentFund,
                    StartDate = item.StartDate,
                    ExpireDate = item.ExpireDate,
                    CreatedAt = item.CreatedAt,
                    Status = item.Status
                });
            }
            return View(projectList);
        }
        public ActionResult CreateProject()
        {
            var item = new ProjectCreateViewModel();
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProject(ProjectCreateViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            var project = new Project
            {
                Name = item.Name,
                ApplicationUserId = CurrentUserId,
                Description = item.Description,
                StartDate = item.StartDate,
                ExpireDate = item.ExpireDate,
                Goal = item.Goal,
                CoverImg = item.CoverImg,
                ContentPart1 = item.ContentPart1,
                ContentPart2 = item.ContentPart2
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }
    }
}