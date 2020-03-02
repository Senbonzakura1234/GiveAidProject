using System;
using System.Collections.Generic;
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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public ProjectController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        public ProjectController(
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
        // GET: Project
        public async Task<ActionResult> Index(int? page, int? limit, string start, string end,
            string nameProject, int? status, int? sortBy, int? direct, int? advance, int? view)
        {
            if (view == null || view > 1 || view < 0)
            {
                view = 0;
            }
            if (advance == null || advance > 1 || advance < 0)
            {
                advance = 0;
            }
            if (nameProject.IsNullOrWhiteSpace())
            {
                nameProject = "";
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


            var projects = await _db.Projects.Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate &&
                                                         d.Name.Contains(nameProject)).ToListAsync();

            if (status != null && Enum.IsDefined(typeof(Project.ProjectStatusEnum), status))
            {
                projects = projects.Where(d => d.Status == (Project.ProjectStatusEnum)status).ToList();
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

            var listProduct = new List<Project>();

            switch (sortBy)
            {
                case (int)HelperEnum.ProjectSortEnum.StartDate when direct is (int)HelperEnum.ProjectDirectEnum.Asc:
                    {
                        var dataList = projects.OrderBy(p => p.StartDate).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.StartDate:
                    {
                        var dataList = projects.OrderByDescending(p => p.StartDate).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.ExpireDate when direct is (int)HelperEnum.ProjectDirectEnum.Asc:
                    {
                        var dataList = projects.OrderBy(p => p.ExpireDate).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.ExpireDate:
                    {
                        var dataList = projects.OrderByDescending(p => p.ExpireDate).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.ProjectName when direct is (int)HelperEnum.ProjectDirectEnum.Asc:
                    {
                        var dataList = projects.OrderBy(p => p.Name).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.ProjectName:
                    {
                        var dataList = projects.OrderByDescending(p => p.Name).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.ProcessPercent when direct is (int)HelperEnum.ProjectDirectEnum.Asc:
                    {
                        var dataList = projects.OrderBy(p => p.CurrentFund).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.ProcessPercent:
                    {
                        var dataList = projects.OrderByDescending(p => p.CurrentFund).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.Status when direct is (int)HelperEnum.ProjectDirectEnum.Asc:
                    {
                        var dataList = projects.OrderBy(p => p.Status).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                case (int)HelperEnum.ProjectSortEnum.Status:
                    {
                        var dataList = projects.OrderByDescending(p => p.Status).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
                default:
                    {
                        var dataList = projects.OrderBy(p => p.CreatedAt).ToList();
                        listProduct.AddRange(dataList);
                        break;
                    }
            }

            Debug.WriteLine(startDate + " " + endDate);
            ViewBag.view = view;
            ViewBag.advance = advance;
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.CurrentPage = page ?? 1;
            ViewBag.TotalItem = listProduct.Count;
            ViewBag.Limit = limit ?? 10;
            ViewBag.TotalPage = Math.Ceiling((double)listProduct.Count / (limit ?? 10));
            ViewBag.nameProject = nameProject;
            ViewBag.status = status;

            ViewBag.sortBy = sortBy;
            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)HelperEnum.DonationDirectEnum.Asc ? (int)HelperEnum.DonationDirectEnum.Desc : (int)HelperEnum.DonationDirectEnum.Asc;

            listProduct = listProduct.Skip(((page ?? 1) - 1) * (limit ?? 10)).Take((limit ?? 10)).ToList();


            var data = listProduct.Select(item => new ProjectListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CoverImg = item.CoverImg,
                    Goal = item.Goal,
                    CurrentFund = item.CurrentFund,
                    StartDate = item.StartDate,
                    ExpireDate = item.ExpireDate,
                    CreatedAt = item.CreatedAt,
                    Status = item.Status
                })
                .ToList();
            return View(data);
        }
        public ActionResult CreateProject()
        {
            var item = new ProjectCreateViewModel();

            ViewBag.categoryList = _db.Categories.ToList();

            return View(item);
        }

        private static List<string> ExtractFromBody(string body, string start, string end)
        {
            List<string> matched = new List<string>();

            int indexStart = 0;
            int indexEnd = 0;

            bool exit = false;
            while (!exit)
            {
                indexStart = body.IndexOf(start);

                if (indexStart != -1)
                {
                    indexEnd = indexStart + body.Substring(indexStart).IndexOf(end);

                    matched.Add(start + body.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length) + end);

                    body = body.Substring(indexEnd + end.Length);
                }
                else
                {
                    exit = true;
                }
            }

            return matched;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProject(ProjectCreateViewModel item)
        {
            ViewBag.categoryList = _db.Categories.ToList();

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var liststring = ExtractFromBody(item.ListImg, "https://", ".png");
            
            var project = new Project
            {
                Name = item.Name,
                ApplicationUserId = CurrentUserId,
                Description = item.Description,
                StartDate = item.StartDate,
                ExpireDate = item.ExpireDate,
                Goal = item.Goal,
                CoverImg = item.CoverImg,
                ContentPart1 = item.ContentPart1,
                ContentPart2 = item.ContentPart2,
                CategoryId = item.CategoryId,
                ReceiverEmail = item.ReceiverEmail
            };
            

            var listImg = new List<ProjectImage>();
            foreach (var VARIABLE in liststring)
            {
                listImg.Add(new ProjectImage
                {
                    ProjectId = project.Id,
                    Url = VARIABLE,
                    Description = project.Name + " image"
                });
            }

            project.ProjectImages = listImg;
            _db.Projects.Add(project);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult EditProject(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var res = _db.Projects.Find(id);

            if (res == null)
            {
                return HttpNotFound();
            }

            var data = new EditProjectStatusViewModel
            {
                Id = res.Id,
                Status = res.Status
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(EditProjectStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = _db.Projects.Find(model.Id);

            res.Status = model.Status;

            _db.Entry(res).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
    }
}