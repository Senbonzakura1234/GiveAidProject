using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
using Microsoft.Ajax.Utilities;

namespace GiveAidCharity.Controllers
{
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Donations
        public async Task<ActionResult> Index(int? page, int? limit, string start, string end, 
            string nameProject, double? minAmount, double? maxAmount, int? status, int? method,
            int? sortBy, int? direct, int? advance, int? view)
        {
            if (view == null || view > 1 || view < 0)
            {
                view = 0;
            }
            if (advance == null || advance > 1 || advance < 0)
            {
                advance = 0;
            }
            if (minAmount == null)
            {
                minAmount = 0;
            }
            if (maxAmount == null)
            {
                maxAmount = 100;
            }
            if (nameProject.IsNullOrWhiteSpace())
            {
                nameProject = "";
            }
            Debug.WriteLine(start + " " + end);
            if (string.IsNullOrWhiteSpace(start) && !HelperMethod.CheckValidDate(start))
            {
                start = HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow).AddYears(-1).ToString("yyyy-MM-dd");
            }
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(end) && !HelperMethod.CheckValidDate(end))
            {
                end = HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow).ToString("yyyy-MM-dd");
            }
            var endDate = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var donations = await _db.Donations.Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate && 
                                                           d.Project.Name.Contains(nameProject) ).ToListAsync();

            donations = donations.Where(p => p.Amount <= maxAmount && p.Amount >= minAmount).ToList();

            if (status != null && Enum.IsDefined(typeof(Donation.DonationStatusEnum), status))
            {
                donations = donations.Where(d => d.Status == (Donation.DonationStatusEnum) status).ToList();
            }
            else
            {
                status = 20;
            }

            if (method != null && Enum.IsDefined(typeof(Donation.PaymentMethodEnum), method))
            {
                donations = donations.Where(d => d.PaymentMethod == (Donation.PaymentMethodEnum) method).ToList();
            }
            else
            {
                method = 20;
            }

            
            if (sortBy == null || !Enum.IsDefined(typeof(HelperEnum.DonationSortEnum), sortBy))
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }

            if (direct == null || !Enum.IsDefined(typeof(HelperEnum.DonationDirectEnum), direct))
            {
                Debug.WriteLine(direct);
                direct = 0;
            }

            //sorting
            
            var listDonation = new List<Donation>();

            switch (sortBy)
            {
                case (int)HelperEnum.DonationSortEnum.CreatedAt when direct is (int)HelperEnum.DonationDirectEnum.Asc:
                {
                    var dataList = donations.OrderBy(p => p.CreatedAt).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.CreatedAt:
                {
                    var dataList = donations.OrderByDescending(p => p.CreatedAt).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int) HelperEnum.DonationSortEnum.UserName when direct is (int)HelperEnum.DonationDirectEnum.Asc:
                {
                    var dataList = donations.OrderBy(d => d.ApplicationUser.UserName).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int) HelperEnum.DonationSortEnum.UserName:
                {
                    var dataList = donations.OrderByDescending
                        (d => d.ApplicationUser.UserName).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.Amount when direct is (int)HelperEnum.DonationDirectEnum.Asc:
                {
                    var dataList = donations.OrderBy(p => p.Amount).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.Amount:
                {
                    var dataList = donations.OrderByDescending(p => p.Amount).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.PaymentMethod when direct is (int)HelperEnum.DonationDirectEnum.Asc:
                {
                    var dataList = donations.OrderBy(p => p.PaymentMethod).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.PaymentMethod:
                {
                    var dataList = donations.OrderByDescending(p => p.PaymentMethod).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.Status when direct is (int)HelperEnum.DonationDirectEnum.Asc:
                {
                    var dataList = donations.OrderBy(p => p.Status).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.Status:
                {
                    var dataList = donations.OrderByDescending(p => p.Status).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.ProjectName when direct is (int)HelperEnum.DonationDirectEnum.Asc:
                {
                    var dataList = donations.OrderBy(p => p.Project.Name).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                case (int)HelperEnum.DonationSortEnum.ProjectName:
                {
                    var dataList = donations.OrderByDescending(p => p.Project.Name).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
                default:
                {
                    var dataList = donations.OrderBy(p => p.CreatedAt).ToList();
                    listDonation.AddRange(dataList);
                    break;
                }
            }

            Debug.WriteLine(startDate + " " + endDate);
            ViewBag.view = view;
            ViewBag.advance = advance;
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.CurrentPage = page ?? 1;
            ViewBag.TotalItem = listDonation.Count;
            ViewBag.Limit = limit ?? 10;
            ViewBag.TotalPage = Math.Ceiling((double)listDonation.Count / (limit ?? 10));
            ViewBag.nameProject = nameProject;
            ViewBag.minAmount = minAmount;
            ViewBag.maxAmount = maxAmount;
            ViewBag.status = status;
            ViewBag.method = method;

            ViewBag.sortBy = sortBy;
            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)HelperEnum.DonationDirectEnum.Asc ? (int)HelperEnum.DonationDirectEnum.Desc : (int)HelperEnum.DonationDirectEnum.Asc;

            listDonation = listDonation.Skip(((page ?? 1) - 1) * (limit ?? 10)).Take((limit ?? 10)).ToList();

            var data = listDonation.Select(item => new DonationListViewModel
                {
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    Amount = item.Amount,
                    ProjectId = item.ProjectId,
                    UserName = item.ApplicationUser.UserName,
                    PaymentMethod = item.PaymentMethod,
                    Avatar = item.ApplicationUser.Avatar,
                    UserId = item.ApplicationUserId,
                    Id = item.Id,
                    ProjectName = item.Project.Name
                })
                .ToList();
            return View(data);
        }

        //// GET: Donations/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var donation = await _db.Donations.FindAsync(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            return View(donation);
        }

        public async Task<ActionResult> EditStatus(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var donation = await _db.Donations.FindAsync(id);
            if (donation == null)
            {
                return HttpNotFound();
            }

            var data = new EditStatusDonationViewModel
            {
                Id = donation.Id,
                Status = donation.Status
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStatus(EditStatusDonationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var res = _db.Donations.Find(model.Id);
            if (res == null)
            {
                return HttpNotFound();
            }

            res.Status = model.Status;
            res.UpdatedAt = HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow);
            _db.Entry(res).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
        //// GET: Donations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName");
        //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ApplicationUserId");
        //    return View();
        //}

        //// POST: Donations/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ApplicationUserId,ProjectId,Amount,PaymentMethod,Status,CreatedAt,UpdatedAt,DeletedAt")] Donation donation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Donations.Add(donation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", donation.ApplicationUserId);
        //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ApplicationUserId", donation.ProjectId);
        //    return View(donation);
        //}

        //// GET: Donations/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Donation donation = db.Donations.Find(id);
        //    if (donation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", donation.ApplicationUserId);
        //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ApplicationUserId", donation.ProjectId);
        //    return View(donation);
        //}

        //// POST: Donations/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ApplicationUserId,ProjectId,Amount,PaymentMethod,Status,CreatedAt,UpdatedAt,DeletedAt")] Donation donation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(donation).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", donation.ApplicationUserId);
        //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ApplicationUserId", donation.ProjectId);
        //    return View(donation);
        //}

        //// GET: Donations/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Donation donation = db.Donations.Find(id);
        //    if (donation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(donation);
        //}

        //// POST: Donations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Donation donation = db.Donations.Find(id);
        //    db.Donations.Remove(donation);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
