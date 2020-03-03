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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public BlogController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        public BlogController(
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
        // GET: Blog
        public async Task<ActionResult> Index(int? page, int? limit, string start, string end,
            string title, int? status, int? sortBy, int? direct, int? advance)
        {
            if (advance == null || advance > 1 || advance < 0)
            {
                advance = 0;
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                title = "";
            }

            Debug.WriteLine(start + " " + end);
            if (string.IsNullOrWhiteSpace(start) && !HelperMethod.CheckValidDate(start))
            {
                start = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            }
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(end) && !HelperMethod.CheckValidDate(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var endDate = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            var blogs = await _db.Blogs.Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate &&
                                                         d.Title.Contains(title)).ToListAsync();

            if (status != null && Enum.IsDefined(typeof(Blog.BlogStatusEnum), status))
            {
                blogs = blogs.Where(d => d.Status == (Blog.BlogStatusEnum)status).ToList();
            }
            else
            {
                status = 20;
            }


            if (sortBy == null || !Enum.IsDefined(typeof(HelperEnum.ProjectSortEnum), sortBy))
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }

            if (direct == null || !Enum.IsDefined(typeof(HelperEnum.ProjectDirectEnum), direct))
            {
                Debug.WriteLine(direct);
                direct = 0;
            }

            var listBlog = new List<DashBoardBlogListViewModel>();
            foreach (var item in blogs)
            {
                listBlog.Add(new DashBoardBlogListViewModel
                {
                    Id = item.Id,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    Vote = await HelperMethod.RatingCount(item.Id),
                    Username = item.ApplicationUser.UserName,
                    UserId = item.ApplicationUserId,
                    Title = item.Title,
                    CategoryId = item.CategoryId,
                    Comment = item.BlogComments.Count(c => c.Status == BlogComment.BlogCommentStatusEnum.Published),
                    CategoryName = item.Category.Name
                });
            }
            var data = new List<DashBoardBlogListViewModel>();

            switch (sortBy)
            {
                case (int)HelperEnum.BlogSortEnum.CreatedAt when direct is (int)HelperEnum.BlogDirectEnum.Asc:
                    {
                        var dataList = listBlog.OrderBy(p => p.CreatedAt).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.CreatedAt:
                    {
                        var dataList = listBlog.OrderByDescending(p => p.CreatedAt).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Comment when direct is (int)HelperEnum.BlogDirectEnum.Asc:
                    {
                        var dataList = listBlog.OrderBy(p => p.Comment).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Comment:
                    {
                        var dataList = listBlog.OrderByDescending(p => p.Comment).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Vote when direct is (int)HelperEnum.BlogDirectEnum.Asc:
                    {
                        var dataList = listBlog.OrderBy(p => p.Vote).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Vote:
                    {
                        var dataList = listBlog.OrderByDescending(p => p.Vote).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Title when direct is (int)HelperEnum.BlogDirectEnum.Asc:
                    {
                        var dataList = listBlog.OrderBy(p => p.Title).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Title:
                    {
                        var dataList = listBlog.OrderByDescending(p => p.Title).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Status when direct is (int)HelperEnum.BlogDirectEnum.Asc:
                    {
                        var dataList = listBlog.OrderBy(p => p.Status).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.BlogSortEnum.Status:
                    {
                        var dataList = listBlog.OrderByDescending(p => p.Status).ToList();
                        data.AddRange(dataList);
                        break;
                    }
                default:
                    {
                        var dataList = listBlog.OrderBy(p => p.CreatedAt).ToList();
                        data.AddRange(dataList);
                        break;
                    }
            }


            Debug.WriteLine(startDate + " " + endDate);
            ViewBag.advance = advance;
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.CurrentPage = page ?? 1;
            ViewBag.TotalItem = data.Count;
            ViewBag.Limit = limit ?? 10;
            ViewBag.TotalPage = Math.Ceiling((double)data.Count / (limit ?? 10));
            ViewBag.BlogTitle = title;
            ViewBag.status = status;

            ViewBag.sortBy = sortBy;
            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)HelperEnum.DonationDirectEnum.Asc ? (int)HelperEnum.DonationDirectEnum.Desc : (int)HelperEnum.DonationDirectEnum.Asc;

            data = data.Skip(((page ?? 1) - 1) * (limit ?? 10)).Take((limit ?? 10)).ToList();
            return View(data);
        }
    }
}