using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;

namespace GiveAidCharity.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(_db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var listBlog = _db.Blogs.OrderByDescending(b => b.UpdatedAt).Where(b => b.CategoryId == id).Select(b => new BlogListViewModel
            {
                Id = b.Id,
                CreatedAt = b.CreatedAt,
                Avatar = b.ApplicationUser.Avatar,
                Username = b.ApplicationUser.UserName,
                Vote = b.Votes.Count(v => v.Status == Vote.VoteStatusEnum.UpVote) - b.Votes.Count(v => v.Status == Vote.VoteStatusEnum.DownVote),
                Description = b.ContentPart1,
                UserId = b.ApplicationUserId,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                Title = b.Title,
                Comment = b.BlogComments.Count
            }).Take(5).ToList();

            var listProject = _db.Projects.OrderByDescending(p => p.UpdatedAt).Where(b => b.CategoryId == id).Select(p =>
                new ProjectListViewModel
                {
                    CreatedAt = p.CreatedAt,
                    Status = p.Status,
                    Name = p.Name,
                    CurrentFund = p.CurrentFund,
                    Id = p.Id,
                    Goal = p.Goal,
                    CoverImg = p.CoverImg,
                    ExpireDate = p.ExpireDate,
                    StartDate = p.StartDate
                }).ToList();

            ViewBag.listBlog = listBlog;
            ViewBag.listProject = listProject;
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CreatedAt,UpdatedAt,DeletedAt,Status")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Categories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CreatedAt,UpdatedAt,DeletedAt,Status")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            category.UpdatedAt = HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow);
            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            category.Status = Category.CategoryStatusEnum.Deleted;
            category.DeletedAt = HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow);
            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
