using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    [Authorize(Roles = "1Administrator, 2Moderator")]
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
        public async Task<ActionResult> Index(int? page, int? limit,
            string userName, int? sortBy, int? direct)
        {

            if (userName.IsNullOrWhiteSpace())
            {
                userName = "";
            }

            var users = await _db.Users.Where(d => d.UserName.Contains(userName)).ToListAsync();
            
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


            if (sortBy == null || !Enum.IsDefined(typeof(HelperEnum.UserSortEnum), sortBy))
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }

            if (direct == null || !Enum.IsDefined(typeof(HelperEnum.UserDirectEnum), direct))
            {
                Debug.WriteLine(direct);
                direct = 0;
            }
            var data = new List<UserListViewModel>();

            switch (sortBy)
            {
                case (int)HelperEnum.UserSortEnum.Role when direct is (int)HelperEnum.UserDirectEnum.Asc:
                    {
                        var dataList = listUser.OrderBy(p => p.Role).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.UserSortEnum.Role:
                    {
                        var dataList = listUser.OrderByDescending(p => p.Role).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.UserSortEnum.UserName when direct is (int)HelperEnum.UserDirectEnum.Asc:
                    {
                        var dataList = listUser.OrderBy(p => p.Username).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.UserSortEnum.UserName:
                    {
                        var dataList = listUser.OrderByDescending(p => p.Username).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.UserSortEnum.TotalDonate when direct is (int)HelperEnum.UserDirectEnum.Asc:
                    {
                        var dataList = listUser.OrderBy(p => p.TotalDonate).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.UserSortEnum.TotalDonate:
                    {
                        var dataList = listUser.OrderByDescending(p => p.TotalDonate).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                default:
                    {
                        var dataList = listUser.OrderBy(p => p.Role).ToList();
                        data.AddRange(dataList);
                        break;
                    }
            }

            ViewBag.CurrentPage = page ?? 1;
            ViewBag.TotalItem = data.Count;
            ViewBag.Limit = limit ?? 10;
            ViewBag.TotalPage = Math.Ceiling((double)data.Count / (limit ?? 10));
            ViewBag.userName = userName;

            ViewBag.sortBy = sortBy;
            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)HelperEnum.DonationDirectEnum.Asc ? (int)HelperEnum.DonationDirectEnum.Desc : (int)HelperEnum.DonationDirectEnum.Asc;

            data = data.Skip(((page ?? 1) - 1) * (limit ?? 10)).Take((limit ?? 10)).ToList();

            return View(data);
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

            var userRole = new UpdateRoleViewModel
            {
                UserId = user.Id,
                Role = GetUserRoleName(user.Id) ?? "1Administrator"
            };
            return View(userRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1Administrator")]
        public async Task<ActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await UserManager.AddToRoleAsync(model.UserId, model.Role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}