using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Razor.Generator;
using System.Web.Security;
using System.Web.Services.Configuration;
using GiveAidCharity.Models;
using GiveAidCharity.Models.Main;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;


namespace GiveAidCharity.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<GiveAidCharity.Models.ApplicationDbContext>
    {
        private const string Blog_Url = "https://api.myjson.com/bins/j9cuo";
        private const string Image_Url = "https://api.myjson.com/bins/7l7uo";
        private const string Id_arr = "https://api.myjson.com/bins/k6los";
        private string[] UserName = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", 
            "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen", 
            "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon", "Michelle", 
            "Laura", "Sarah", "Kimberly", "Deborah", "James", "John", "Robert", "Michael", 
            "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", 
            "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", 
            "Brian", "Ronald", "Anthony", "Kevin", "Jason", "Jeff" };
        private string[] _categoryId =
        {
            "9104390c-469d-4647-aaf0-8c2998d31213",
            "57e2b432-3542-49af-9ec0-704d744b715d",
            "372a6e2a-665f-4bd4-90a2-ed50c44e80b1",
            "68ebea3b-3a01-44b3-a768-7f0adb2cfa25",
            "686808ac-ff5a-4502-96b8-6c76a9c5f78c"
        };

        private string _blogCommentParent =
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s";

        private string _blogCommentChild = 
            "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters";
        private string _projectCommentParent =
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s";

        private string _projectCommentChild =
            "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GiveAidCharity.Models.ApplicationDbContext context)
        {
            //SeedRole(context);
            
            //SeedUser(context);

            //SeedCategories(context);

            //SeedProjects(context);

            //SeedBlogs(context);

            //SeedProjectImages(context);

            //SeedDonations(context);

            //SeedProjectComments(context);

            //SeedBlogComments(context);
            
        }

        public void SeedUser(GiveAidCharity.Models.ApplicationDbContext context)
        {
            for (var i = 0; i < 30; i++)
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = UserName[i],
                    Email = UserName[i] + "@gmail.com",
                    Id = Guid.NewGuid().ToString()
                };
                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "Member");
            }
            for (var j = 31; j < 40; j++)
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = UserName[j],
                    Email = UserName[j] + "@gmail.com",
                    Id = Guid.NewGuid().ToString()
                };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "FundRaiser");
            }
            for (var k = 41; k < 50; k++)
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = UserName[k],
                    Email = UserName[k] + "@gmail.com",
                    Id = Guid.NewGuid().ToString()
                };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "Volunteer");
            }
        }

        public void SeedRole(GiveAidCharity.Models.ApplicationDbContext context)
        {
            //add User Roles
            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Administrator" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Moderator" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "FundRaiser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "FundRaiser" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Volunteer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Volunteer" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Member" };

                manager.Create(role);
            }
        }

        public void SeedCategories(ApplicationDbContext context)
        {
            var listCategory = new List<Category>();
            listCategory.AddRange(new List<Category>
            {
                new Category {Id =_categoryId[0], Name = "Humanitarian", Description = "crimes of war, terrorism"},
                new Category {Id =_categoryId[1], Name = "Natural disaster", Description = "Save human in disaster"},
                new Category {Id =_categoryId[2], Name = "Education", Description = "Give the change education"},
                new Category {Id =_categoryId[3], Name = "Environment", Description = "Save the environment"},
                new Category {Id =_categoryId[4], Name = "Culture", Description = "Save the cultural building"}
            });
            context.Categories.AddRange(listCategory);
            context.SaveChanges();
        }

        public void SeedProjects(ApplicationDbContext context)
        {
            try
            {
                var lsProjects = GetJsonData<Article>(Blog_Url);
                var lsImages = GetJsonData<ArticleImage>(Image_Url);
                var dictionaryId = IdGuid();
                //Lấy List Id của member
                var lsMemberFund = (from tb1 in context.Users
                                    from tb2 in tb1.Roles
                                    join tb3 in context.Roles on tb2.RoleId equals tb3.Id
                                    where tb3.Name == "FundRaiser"
                                    select tb1.Id).ToList();
                var rdn = new Random();
                var listProjects = lsProjects.Select(f => new Project
                {
                    Id = dictionaryId[f.projectid],
                    ApplicationUserId = lsMemberFund[rdn.Next(1, lsMemberFund.Count - 1)],
                    Name = f.projectname,
                    CategoryId = _categoryId[rdn.Next(0, 4)],
                    Description = f.projectname,
                    CoverImg = f.CoverImg,
                    Goal = rdn.Next(100, 1000),
                    ContentPart1 = f.contentpart1,
                    ContentPart2 = f.contentpart2,
                    StartDate = new DateTime(2019, 02, 14).AddDays(rdn.Next(1, 340)),
                    CurrentFund = 0,
                    Status = 0,
                    DeletedAt = null,
                })
                    .ToList();
                foreach (var v in listProjects)
                {
                    v.CreatedAt = v.StartDate.AddDays(2);
                    v.UpdatedAt = v.StartDate.AddDays(-2);
                    v.ExpireDate = v.StartDate.AddDays(10);
                    v.ReceiverEmail = context.Users.Find(v.ApplicationUserId).Email.ToString();
                }
                context.Projects.AddRange(listProjects);
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void SeedBlogs(ApplicationDbContext context)
        {
            var lsProjects = GetJsonData<Article>(Blog_Url);
            var lsImages = GetJsonData<ArticleImage>(Image_Url);
            var dictionaryId = IdGuid();
            var rdn = new Random();

            var listBlog = lsProjects.Select(f => new Blog()
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = context.Projects.AsEnumerable()
                        .Where(p => p.Id.Contains($"{dictionaryId[f.projectid]}"))
                        .Select(p => p.ApplicationUserId)
                        .SingleOrDefault(),
                Rss = dictionaryId[f.projectid],
                CategoryId = _categoryId[rdn.Next(0, 4)],
                Title = f.projectname,
                ContentPart1 = f.contentpart1,
                ContentPart2 = f.contentpart2,
                Status = Blog.BlogStatusEnum.Published,
                CreatedAt = context.Projects.AsEnumerable()
                        .Where(p => p.Id.Contains($"{dictionaryId[f.projectid]}"))
                        .Select(p => p.CreatedAt)
                        .SingleOrDefault(),
                UpdatedAt = context.Projects.AsEnumerable()
                        .Where(p => p.Id.Contains($"{dictionaryId[f.projectid]}"))
                        .Select(p => p.CreatedAt)
                        .SingleOrDefault(),
                DeletedAt = null
            })
                .ToList();
            context.Blogs.AddRange(listBlog);
            context.SaveChanges();
        }

        public void SeedProjectImages(ApplicationDbContext context)
        {
            var lsImages = GetJsonData<ArticleImage>(Image_Url);
            var dictionaryId = IdGuid();
            var listImages = lsImages.Select(f => new ProjectImage()
            {

                Id = Guid.NewGuid().ToString(),
                ProjectId = dictionaryId[f.projectid],
                Url = f.imgUrl,
                Description = f.content,
                Status = ProjectImage.ProjectImageStatusEnum.Show,
                CreatedAt = context.Projects.AsEnumerable()
                    .Where(p => p.Id == dictionaryId[f.projectid])
                    .Select(p => p.CreatedAt)
                    .SingleOrDefault(),
                UpdatedAt = context.Projects.AsEnumerable()
                    .Where(p => p.Id == dictionaryId[f.projectid])
                    .Select(p => p.CreatedAt)
                    .SingleOrDefault(),
                DeletedAt = null
            })
                .ToList();

            context.ProjectImages.AddRange(listImages);
            context.SaveChanges();
        }

        public void SeedDonations(ApplicationDbContext context)
        {
            
            var listDonation = new List<Donation>();
            var projectCount = context.Projects.Count();
            //var lsProject = context.Projects.Select(p => new {p.Id, p.ApplicationUserId}).ToList();
            //Lấy List Id của pj
            var lsProjectId = context.Projects.Select(p => p.Id).ToList();
            //Lấy List Id của member
            var lsMemberId = (from tb1 in context.Users
                              from tb2 in tb1.Roles
                              join tb3 in context.Roles on tb2.RoleId equals tb3.Id
                              where tb3.Name == "Member"
                              select tb1.Id).ToList();
            Array values = Enum.GetValues(typeof(Donation.PaymentMethodEnum));
            var rdn = new Random();

            for (int i = 0; i < projectCount; i++)
            {
                var donationPerPj = rdn.Next(15, 20);
                for (int j = 0; j < donationPerPj; j++)
                {
                    var randomPayment = (Donation.PaymentMethodEnum)values.GetValue(rdn.Next(values.Length));
                    // var newSpan = new DateTime(0, 0, new Random().Next(0, 8));
                    string projectId = lsProjectId[i];
                    listDonation.Add(new Donation()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ApplicationUserId = lsMemberId[j],
                        ProjectId = lsProjectId[i],
                        Amount = rdn.Next(1, 100), //cost mỗi donation 1$-100$
                        PaymentMethod = randomPayment,
                        Status = (Donation.DonationStatusEnum)rdn.Next(1, 3),
                        CreatedAt = context.Projects.Where(o => o.Id == projectId).Select(p => p.StartDate).SingleOrDefault().AddDays(rdn.Next(0, 8)),
                        DeletedAt = null
                    });
                }

                foreach (var d in listDonation)
                {
                    d.UpdatedAt = d.CreatedAt;
                }

                context.Donations.AddRange(listDonation);
                context.SaveChanges();
                listDonation.Clear();
            }
        }

        public void SeedProjectComments(ApplicationDbContext context)
        {
            var listComment = new List<ProjectComment>();
            var listChildComment = new List<ProjectComment>();
            var dictionaryId = IdGuid();
            for (int i = 1; i <= 30; i++)
            {
                string projectId = dictionaryId[$"{i}"];
                for (int j = 0; j < 3; j++)
                {
                    var spanTime = new Random().Next(1, 3);
                    listComment.Add(new ProjectComment
                    {
                        Id = Guid.NewGuid().ToString(),
                        ApplicationUserId = context.Projects.Where(p => p.Id == projectId).Select(p => p.ApplicationUserId).SingleOrDefault(),
                        ProjectId = projectId,
                        ParentId = null,
                        Content = _projectCommentParent,
                        CreatedAt = context.Projects.Where(p => p.Id == projectId).Select(p => p.StartDate).SingleOrDefault().AddDays(spanTime),
                        DeletedAt = null
                    });
                }

            }
            foreach (var c in listComment)
            {
                c.UpdatedAt = c.CreatedAt;
            }
            context.ProjectComments.AddRange(listComment);
            context.SaveChanges();
            //var listComment = context.ProjectComments.Select(row => row).ToList();
            foreach (var c in listComment)
            {
                string projectId = c.ProjectId;
                string parentId = c.Id;
                listChildComment.Add(new ProjectComment
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationUserId = context.Projects.Where(p => p.Id == projectId).Select(p => p.ApplicationUserId).SingleOrDefault(),
                    ProjectId = projectId,
                    ParentId = parentId,
                    Content = _projectCommentChild,
                    CreatedAt = context.ProjectComments.Where(p => p.Id == parentId).Select(p => p.CreatedAt).SingleOrDefault(),
                    DeletedAt = null
                });
            }
            foreach (var p in listChildComment)
            {
                p.UpdatedAt = p.CreatedAt;
            }
            context.ProjectComments.AddRange(listChildComment);
            context.SaveChanges();
        }

        public void SeedBlogComments(ApplicationDbContext context)
        {
            var listBlogComments = new List<BlogComment>();
            var listChildBlogComments = new List<BlogComment>();
            var listBlog = context.Blogs.ToList();

            var rdn = new Random();
            Array values = Enum.GetValues(typeof(BlogComment.BlogCommentStatusEnum));
            foreach (var p in listBlog)
            {

                    for (int j = 0; j < 3; j++)
                    {
                        var spanTime = new Random().Next(1, 3);
                        var randomCommentEnum =
                            (BlogComment.BlogCommentStatusEnum) values.GetValue(rdn.Next(values.Length));
                        listBlogComments.Add(new BlogComment
                        {
                            Id = Guid.NewGuid().ToString(),
                            ApplicationUserId = p.ApplicationUserId,
                            BlogId = p.Id,
                            ParentId = null,
                            Content = _blogCommentParent,
                            Status = (BlogComment.BlogCommentStatusEnum) rdn.Next(0, 2),
                            CreatedAt = p.CreatedAt.AddDays(spanTime)
                        });
                    }
            }

            foreach (var c in listBlogComments)
            {
                c.UpdatedAt = c.CreatedAt;
            }

            context.BlogComments.AddRange(listBlogComments);
            context.SaveChanges();


            foreach (var c in listBlogComments)
            {
                listChildBlogComments.Add(new BlogComment
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationUserId = c.ApplicationUserId,
                    BlogId = c.BlogId,
                    ParentId = c.Id,
                    Content = _blogCommentChild,
                    CreatedAt = c.CreatedAt,
                    DeletedAt = null
                });
            }

            foreach (var p in listChildBlogComments)
            {
                p.UpdatedAt = p.CreatedAt;
            }

            context.BlogComments.AddRange(listChildBlogComments);
            context.SaveChanges();
        }

        // Mapping (int) ProjectId in Json file with (GuiId) ProjectId to insert (Guid) ProjectId into db
        public Dictionary<string, string> IdGuid()
        {
            IDictionary<string, string> dicId = new Dictionary<string, string>();
            var lsIdGuid = GetJsonData<Id>(Id_arr);
            var lsId = lsIdGuid.Select(f => new Id
            {
                num = f.num,
                gu = f.gu
            }).ToList();
            foreach (var v in lsId)
            {
                dicId.Add($"{v.num}", $"{v.gu}");
            }
            return (Dictionary<string, string>) dicId;
        }
        //Get Json Data
        private List<T> GetJsonData<T>(string url) where T : new()
        {
            var client = new HttpClient();
            var responseContent = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<T>>(responseContent);
            return list;
        }

    }

    public class Article
    { 
        public string projectid { get; set; }
        public string CoverImg { get; set; }
        public string projectname { get; set; }
        public string contentpart1 { get; set; }
        public string contentpart2 { get; set; }
    }

    public class ArticleImage
    {
        public string projectid { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
        public string content { get; set; }
    }

    public class Id
    {
        public string num { get; set; }
        public string gu { get; set; }
    }

}
