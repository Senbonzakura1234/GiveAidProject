using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GiveAidCharity.Models;

namespace GiveAidCharity.Controllers
{
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Donations
        public async Task<ActionResult> Index(int? page, int? limit, string start, string end)
        {
            Debug.WriteLine(start + " " + end);
            if (string.IsNullOrWhiteSpace(start) && !CheckValidDate(start))
            {
                start = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            }
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(end) && !CheckValidDate(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var endDate = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var donations = await _db.Donations.Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate).ToListAsync();

            Debug.WriteLine(startDate + " " + endDate);
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.CurrentPage = page ?? 1;
            ViewBag.Limit = limit ?? 10;
            ViewBag.TotalPage = Math.Ceiling((double) donations.Count / (limit ?? 10));

            donations = donations.OrderByDescending(d => d.CreatedAt).Skip(((page ?? 1) - 1) * (limit ?? 10)).Take((limit ?? 10)).ToList();

            return View(donations.ToList());
        }

        //// GET: Donations/Details/5
        //public ActionResult Details(string id)
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
        private static bool CheckValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}
