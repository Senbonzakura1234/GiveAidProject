using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
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

        public ActionResult Banner1Data()
        {
            var start = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var data = _db.Donations.Where(d => d.CreatedAt >= startDate).Sum(d => d.Amount);
            data = Math.Round(data, 2);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Banner2Data()
        {
            var start = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var data = _db.Donations.Where(d => d.CreatedAt >= startDate).ToList().Count();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Banner3Data()
        {
            var data = _db.Donations.ToList();

            var amount = data.Average(d => d.Amount);
            amount = Math.Round(amount, 2);
            return Json(amount, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> GetDonations(string fromDate, string toDate)
        {
            Debug.WriteLine(fromDate + " " + toDate);
            if (string.IsNullOrWhiteSpace(fromDate) && !HelperMethod.CheckValidDate(fromDate))
            {
                fromDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            }
            var startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(toDate) && !HelperMethod.CheckValidDate(toDate))
            {
                toDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var list = await _db.Donations.Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate).ToListAsync();

            var directBankPerMonth = list.Where(d => d.PaymentMethod == Donation.PaymentMethodEnum.DirectBankTransfer)
                .OrderBy(d => d.CreatedAt).GroupBy(d => new
            {
                d.CreatedAt.Month,
                d.CreatedAt.Year
            }).Select(d => new
            {
                Amount = d.Sum(donation => donation.Amount),
                d.FirstOrDefault()?.CreatedAt.Month,
                d.FirstOrDefault()?.CreatedAt.Year,
            }).ToList();

            var vnPayPerMonth = list.Where(d => d.PaymentMethod == Donation.PaymentMethodEnum.VnPay)
                .OrderBy(d => d.CreatedAt).GroupBy(d => new
            {
                d.CreatedAt.Month,
                d.CreatedAt.Year
            }).Select(d => new
            {
                Amount = d.Sum(donation => donation.Amount),
                d.FirstOrDefault()?.CreatedAt.Month,
                d.FirstOrDefault()?.CreatedAt.Year,
            }).ToList();

            var paypalPerMonth = list.Where(d => d.PaymentMethod == Donation.PaymentMethodEnum.Paypal)
                .OrderBy(d => d.CreatedAt).GroupBy(d => new
            {
                d.CreatedAt.Month,
                d.CreatedAt.Year
            }).Select(d => new
            {
                Amount = d.Sum(donation => donation.Amount),
                d.FirstOrDefault()?.CreatedAt.Month,
                d.FirstOrDefault()?.CreatedAt.Year,
            }).ToList();

            var amountPerMonth = list.OrderBy(d => d.CreatedAt).GroupBy(d => new
            {
                d.CreatedAt.Month,
                d.CreatedAt.Year
            }).Select(d => new
            {
                Amount = d.Sum(donation => donation.Amount),
                d.FirstOrDefault()?.CreatedAt.Month,
                d.FirstOrDefault()?.CreatedAt.Year
            }).ToList();

            
            return Json(new
            {
                directBankPerMonth,
                vnPayPerMonth,
                paypalPerMonth,
                amountPerMonth
            }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public async Task<ActionResult> GetPaymentMethod()
        {
            var list = await _db.Donations.ToListAsync();
            var paymentMethod = list.GroupBy(d => new
            {
                d.PaymentMethod
            }).Select(d => new
            {
                d.FirstOrDefault()?.PaymentMethod,
                Quantity = d.Count()
            }).ToList();
            return Json(new
            {
                paymentMethod
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