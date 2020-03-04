using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GiveAidCharity.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleManager<ApplicationUserRole> _roleManager;

        public RolesController()
        {
            var roleStore = new RoleStore<ApplicationUserRole>(_db);
            _roleManager = new RoleManager<ApplicationUserRole>(roleStore);
        }

        public async Task<ActionResult> Index()
        {
            var roles = await _db.Roles.OrderBy(r => r.Users.Count).ToListAsync();
            var rolesList = roles.Select(item => 
                new RoleListViewModel
                {
                    Name = item.Name, 
                    Description = item.Name, 
                    UserCount = item.Users.Count
                }).ToList();
            return View(rolesList);
        }
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(ApplicationUserRole role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(role);
        }
    }
}