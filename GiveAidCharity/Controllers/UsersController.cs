using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public UsersController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        public UsersController(
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
        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = await _db.Users.ToListAsync();
            var listUser = users.Select(item => new UserListViewModel
                {
                    Id = item.Id,
                    Status = item.Status,
                    Username = item.UserName,
                    Avatar = item.Avatar,
                    Gender = item.Gender,
                    TotalDonate = item.Donations.Where(d => d.Status == Donation.DonationStatusEnum.Success)
                        .Sum(d => d.Amount),
                    Role = GetUserRoleName(item.Roles.FirstOrDefault()?.RoleId) ?? "8NotSet"
            })
                .ToList();
            listUser = listUser.OrderBy(u => u.Role).ToList();
            return View(listUser);
        }

        public string GetUserRoleName(string id)
        {
            return _db.Roles.Find(id)?.Name;
        }

        public async Task<ActionResult> UpdateRole(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userRole = new UpdateRoleViewModal
            {
                UserId = user.Id,
                Role = GetUserRoleName(user.Id) ?? "1Administrator"
            };
            return View(userRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateRole(UpdateRoleViewModal modal)
        {
            if (!ModelState.IsValid)
            {
                return View(modal);
            }
            await UserManager.AddToRoleAsync(modal.UserId, modal.Role);
            return RedirectToAction("Index");
        }
    }
}