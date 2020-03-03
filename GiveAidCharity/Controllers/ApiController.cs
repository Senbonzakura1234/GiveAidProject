using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
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

        public async Task<ActionResult> Banner1Data()
        {
            var start = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var list = await _db.Donations.Where(d => d.CreatedAt >= startDate && d.Status == Donation.DonationStatusEnum.Success).ToListAsync();
            
            
            var data = list.Count == 0 ? "0" : $"{Math.Round(list.Sum(d => d.Amount), 2):n}"; 
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Banner2Data()
        {
            var list = await _db.Donations.Where(d => d.Status == Donation.DonationStatusEnum.Success).ToListAsync();
            var data = list.Count == 0 ? "0" : $"{Math.Round(list.Sum(d => d.Amount), 2):n}";
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Banner3Data()
        {
            var data = await _db.Donations.Where(d => d.Status == Donation.DonationStatusEnum.Success).ToListAsync();
            var amount = data.Count != 0? $"{Math.Round(data.Average(d => d.Amount), 2):n}" : "0";
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AjaxFindByBlogTitle(string title)
        {
            if (title.IsNullOrWhiteSpace())
            {
                title = "";
            }
            var list = _db.Blogs.Where(p => p.Title.Contains(title)).Select(p => p.Title);

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
            var listProject = data.Select(item => new projectJsonModel {name = item.Name, currentFund = item.CurrentFund, startDate = item.StartDate}).ToList();
            return Json(new
            {
                listProject
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Vote(string userId, string blogId, int? status)
        {
            if (status == null || !Enum.IsDefined(typeof(Vote.VoteStatusEnum), status))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentUserid = User.Identity.GetUserId();
            if (currentUserid == null || userId == null || !currentUserid.Equals(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var votes = await _db.Votes.Where(v => v.ApplicationUserId == userId && v.BlogId == blogId).ToListAsync();
            if (votes.Count != 0)
            {
                votes = votes.Where(v => v.Status != Models.Main.Vote.VoteStatusEnum.Neutral).ToList();
                if (votes.Count > 0)
                {
                    foreach (var item in votes)
                    {
                        item.Status = Models.Main.Vote.VoteStatusEnum.Neutral;
                        item.UpdatedAt = DateTime.Now;
                        _db.Entry(item).State = EntityState.Modified;
                    }
                }
            }
            Debug.WriteLine(status);
            Debug.WriteLine((Vote.VoteStatusEnum)status);
            var vote = new Vote
            {
                ApplicationUserId = userId,
                BlogId = blogId,
                Status = (Vote.VoteStatusEnum) status
            };
            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Follow(string userId, string projectId, int? status)
        {
            if (status == null || !Enum.IsDefined(typeof(Follow.FollowStatusEnum), status))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentUserid = User.Identity.GetUserId();
            if (currentUserid == null || userId == null || !currentUserid.Equals(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var follows = await _db.Follows.Where(v => v.ApplicationUserId == userId && v.ProjectId == projectId).ToListAsync();
            if (follows.Count != 0)
            {
                follows = follows.Where(v => v.Status != Models.Main.Follow.FollowStatusEnum.Unfollowed).ToList();
                if (follows.Count > 0)
                {
                    foreach (var item in follows)
                    {
                        item.Status = Models.Main.Follow.FollowStatusEnum.Unfollowed;
                        item.UpdatedAt = DateTime.Now;
                        _db.Entry(item).State = EntityState.Modified;
                    }
                }
            }
            var follow = new Follow
            {
                ApplicationUserId = userId,
                ProjectId = projectId,
                Status = (Follow.FollowStatusEnum) status
            };
            _db.Follows.Add(follow);
            await _db.SaveChangesAsync();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetRating(string blogId)
        {
            var rating = await HelperMethod.RatingCount(blogId);
            return Json(new { rating }, JsonRequestBehavior.AllowGet);
        }
    }
}