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

        public async Task<ActionResult> Index()
        {
            var homepage = new HomeViewModel();
            var projectsList = await _db.Projects
                .Where(d => d.Status == Project.ProjectStatusEnum.Ongoing)
                .ToListAsync();
            var causesList = projectsList.Select(t => new CausesListViewModel
            {
                Id = t.Id,
                Name = t.Name,
                CurrentFund = t.CurrentFund,
                Goal = t.Goal,
                Description = t.Description,
                StartDate = t.StartDate,
                ExpireDate = t.ExpireDate,
                FollowCount = t.Follows.Count,
                CoverImg = t.CoverImg
            }).Take(6).ToList();
            var donations = await _db.Donations.Where(d => d.Project.Status != Project.ProjectStatusEnum.Canceled
                                               && d.Project.Status != Project.ProjectStatusEnum.Pending
                                               && d.Status == Donation.DonationStatusEnum.Success
                                               ).Take(9).ToListAsync();
            var listDonations = (from item in donations let cause = new CausesListViewModel { Id = item.ProjectId, Name = item.Project.Name }
                                 select new DonationsListViewModel
                                 {
                                     UserId = item.ApplicationUserId,
                                     Avatar = item.ApplicationUser.Avatar,
                                     Username = item.ApplicationUser.UserName,
                                     Amount = item.Amount,
                                     Cause = cause
                                 }).ToList();
            listDonations = listDonations.OrderByDescending(p => p.DonateDate).ToList();
            listDonations = listDonations.Take(6).ToList();
            homepage.CausesList = causesList;
            homepage.DonationList = listDonations;
            return View(homepage);
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
            var projectsList = await _db.Projects
                .Where(p => p.Status == Project.ProjectStatusEnum.Ongoing
                            || p.Status == Project.ProjectStatusEnum.Success)
                .ToListAsync();
            var causesList = projectsList.Select(t => new CausesListViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    CurrentFund = t.CurrentFund,
                    Goal = t.Goal,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    ExpireDate = t.ExpireDate,
                    FollowCount = t.Follows.Count,
                    CoverImg = t.CoverImg
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
            if (project == null 
                || project.Status != Project.ProjectStatusEnum.Ongoing
                || project.Status != Project.ProjectStatusEnum.Success)
            {
                return HttpNotFound();
            }

            var host = await UserManager.FindByIdAsync(project.ApplicationUserId);
            var images = await _db.ProjectImages.Where(p => p.ProjectId == id && p.Description != null).ToListAsync();
            var comments = await _db.ProjectComments.Where(p => p.ProjectId == id).ToListAsync();
            var cause = new CausesDetailViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Status = project.Status,
                StartDate = project.StartDate,
                ExpireDate = project.ExpireDate,
                Goal = project.Goal,
                CoverImg = project.CoverImg,
                CurrentFund = project.CurrentFund,
                Description = project.Description,
                FollowCount = project.Follows.Count,
                ProjectImages = images,
                ProjectComments = comments,
                ContentPart1 = project.ContentPart1,
                ContentPart2 = project.ContentPart2,
                HostName = host == null? "" : host.UserName,
                HostDescription = host == null ? "" : host.Description,
                HostEmail = host == null ? "" : host.Email,
                HostPhone = host == null ? "" : host.PhoneNumber,
                HostAvatar = host == null ? "" : host.Avatar
            };
            return View(cause);
        }

        public ActionResult SingleTopDonate(string id)
        {
            var listDonate = _db.Donations.Where(d => d.ProjectId == id &&
                                                            d.Status == Donation.DonationStatusEnum.Success).ToList();
            var listDonateViewModel = new List<SingleDonationViewModel>();
            if (listDonate.Count > 0)
            {
                listDonateViewModel.AddRange(listDonate.Select(t => new SingleDonationViewModel
                {
                    Amount = t.Amount,
                    CauseId = t.ProjectId,
                    DonateDate = t.CreatedAt,
                    UserId = t.ApplicationUserId,
                    Username = t.ApplicationUser.UserName,
                    Avatar = t.ApplicationUser.Avatar
                }));
            }

            var listDonateViewModelGroupBy = listDonateViewModel.GroupBy(ld => ld.UserId).Select(ld => new SingleDonationViewModel
            {
                Amount = ld.Sum(b => b.Amount),
                CauseId = ld.FirstOrDefault()?.CauseId,
                // ReSharper disable  PossibleNullReferenceException
                DonateDate = ld.FirstOrDefault().DonateDate,
                UserId = ld.FirstOrDefault()?.UserId,
                Username = ld.FirstOrDefault()?.Username,
                Avatar = ld.FirstOrDefault()?.Avatar
            }).OrderByDescending(ld => ld.Amount).Take(3).ToList();

            return PartialView(listDonateViewModelGroupBy);
        }

        public ActionResult CausesDonate(string id, int? page, int? limit)
        {
            var listDonate = _db.Donations.Where(d => d.ProjectId == id &&
                                                      d.Status == Donation.DonationStatusEnum.Success).ToList();
            var listDonateViewModel = new List<SingleDonationViewModel>();
            if (listDonate.Count > 0)
            {
                listDonateViewModel.AddRange(listDonate.Select(t => new SingleDonationViewModel
                {
                    Amount = t.Amount,
                    CauseId = t.ProjectId,
                    DonateDate = t.CreatedAt,
                    UserId = t.ApplicationUserId,
                    Username = t.ApplicationUser.UserName,
                    Avatar = t.ApplicationUser.Avatar
                }));
            }

            var listDonateViewModelGroupBy = listDonateViewModel.GroupBy(ld => ld.UserId).Select(ld => new SingleDonationViewModel
            {
                Amount = ld.Sum(b => b.Amount),
                CauseId = ld.FirstOrDefault()?.CauseId,
                DonateDate = ld.FirstOrDefault().DonateDate,
                UserId = ld.FirstOrDefault()?.UserId,
                Username = ld.FirstOrDefault()?.Username,
                Avatar = ld.FirstOrDefault()?.Avatar
            }).OrderByDescending(ld => ld.Amount).ToList();

            if(page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 9;
            }

            ViewBag.TotalPage = Math.Ceiling((double)listDonateViewModelGroupBy.Count / limit.Value);
            ViewBag.CurrentPage = page;

            ViewBag.Limit = limit;

            ViewBag.TotalItem = listDonateViewModelGroupBy.Count;

            listDonateViewModelGroupBy = listDonateViewModelGroupBy.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

            return View(listDonateViewModelGroupBy);
        }

        public async Task<ActionResult> Donations()
        {

            var donations = await _db.Donations.Where(d => d.Project.Status != Project.ProjectStatusEnum.Canceled
                                                           && d.Project.Status != Project.ProjectStatusEnum.Pending
                                                           && d.Status == Donation.DonationStatusEnum.Success
                                                           ).Take(9)
                .ToListAsync();
            var listDonations = (from item in donations let cause = new CausesListViewModel {Id = item.ProjectId, Name = item.Project.Name} 
                select new DonationsListViewModel {UserId = item.ApplicationUserId, Avatar = item.ApplicationUser.Avatar,
                    Username = item.ApplicationUser.UserName, Amount = item.Amount, Cause = cause}).ToList();
            return View(listDonations);
        }

        //public async Task<ActionResult> Test()
        //{
        //    var res = await _db.Projects.ToListAsync();
        //    foreach (var item in res)
        //    {
        //        item.CurrentFund = 0;
        //        foreach (var donate in item.Donations)
        //        {
        //            if (donate.Status == Donation.DonationStatusEnum.Success)
        //            {
        //                item.CurrentFund += donate.Amount;
        //            }
        //        }

        //        _db.Entry(item).State = EntityState.Modified;
        //        await _db.SaveChangesAsync();
        //    }

        //    return null;
        //}
        //public async Task<ActionResult> Test()
        //{
        //    var res = await _db.Users.ToArrayAsync();
        //    foreach (var item in res)
        //    {
        //        item.Avatar =
        //            "https://res.cloudinary.com/bangnguyen/image/upload/v1581844808/ProjectCharity/person_1_kvy425.jpg";

        //        _db.Entry(item).State = EntityState.Modified;
        //        await _db.SaveChangesAsync();
        //    }

        //    return null;
        //}
        //public async Task<ActionResult> Test()
        //{
        //    var res = await _db.Projects.ToListAsync();
        //    foreach (var item in res)
        //    {
        //        item.Status = item.CurrentFund >= item.Goal ? 
        //            Project.ProjectStatusEnum.Success : Project.ProjectStatusEnum.Ongoing;
        //        _db.Entry(item).State = EntityState.Modified;
        //        await _db.SaveChangesAsync();
        //    }

        //    return null;
        //}
        [HttpPost]
        public ActionResult Donate(double amount, string projectId)
        {
            return null;
        }
    }
}