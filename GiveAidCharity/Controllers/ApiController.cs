using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    public class ApiController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ApiController()
        {
        }

        public ApiController(
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



        // GET: Api
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AjaxCheckUserName(string username)
        {
            var validationResult = await UserManager.FindByNameAsync(username) == null;
            Debug.WriteLine(validationResult);
            return Json(validationResult);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AjaxCheckEmail(string email)
        {
            var validationResult = await UserManager.FindByEmailAsync(email) == null;
            Debug.WriteLine(validationResult);
            return Json(validationResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AjaxFindByProjectName(string nameProject)
        {
            if (nameProject.IsNullOrWhiteSpace())
            {
                nameProject = "";
            }
            var list = _db.Projects.Where(p => p.Name.Contains(nameProject)).Select(p => p.Name);

            return Json(list);
        }

        [AllowAnonymous]
        public ActionResult GetDonations(DateTime fromDate, DateTime toDate)
        {
            var list = _db.Donations.Where(d => d.CreatedAt >= fromDate && d.CreatedAt <= toDate).ToList();

            var countPerMonth = list.OrderBy(d => d.CreatedAt).GroupBy(d => new
            {
                d.CreatedAt.Month,
                d.CreatedAt.Year
            }).Select(d => new
            {
                Quantity = d.Count(),
                d.FirstOrDefault().CreatedAt.Month,
                d.FirstOrDefault().CreatedAt.Year,
            }).ToList();

            var amountPerMonth = list.OrderBy(d => d.CreatedAt).GroupBy(d => new
            {
                d.CreatedAt.Month,
                d.CreatedAt.Year
            }).Select(d => new
            {
                Amount = d.Sum(donation => donation.Amount),
                d.FirstOrDefault().CreatedAt.Month,
                d.FirstOrDefault().CreatedAt.Year
            }).ToList();

            var PaymentMethod = list.GroupBy(d => new
            {
                d.PaymentMethod
            }).Select(d => new
            {
                d.FirstOrDefault().PaymentMethod,
                Quantity = d.Count()
            }).ToList();
            return Json(new
            {
                countPerMonth,
                amountPerMonth,
                PaymentMethod
            }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public async Task<ActionResult> GetProjectData()
        {
            var data = await _db.Projects.OrderByDescending(p => p.StartDate).Take(10).ToListAsync();
            var listProject = new List<projectJsonModel>();
            foreach (var item in data)
            {
                listProject.Add(new projectJsonModel { name = item.Name, currentFund = item.CurrentFund, startDate = item.StartDate });
            }
            return Json(new
            {
                listProject
            }, JsonRequestBehavior.AllowGet);
        }
    }
}