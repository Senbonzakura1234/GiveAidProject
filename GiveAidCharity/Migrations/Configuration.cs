namespace GiveAidCharity.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<GiveAidCharity.Models.ApplicationDbContext>
    {
        //private const string Project_Url = "";
        //private const string Image_Url = "";
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GiveAidCharity.Models.ApplicationDbContext context)
        {
            ////add User Roles
            //if (!context.Roles.Any(r => r.Name == "Admin"))
            //{
            //    var store = new RoleStore<IdentityRole>(context);
            //    var manager = new RoleManager<IdentityRole>(store);
            //    var role = new IdentityRole { Name = "Admin" };

            //    manager.Create(role);
            //}

            //if (!context.Roles.Any(r => r.Name == "Mod"))
            //{
            //    var store = new RoleStore<IdentityRole>(context);
            //    var manager = new RoleManager<IdentityRole>(store);
            //    var role = new IdentityRole { Name = "Mod" };

            //    manager.Create(role);
            //}

            //if (!context.Roles.Any(r => r.Name == "User"))
            //{
            //    var store = new RoleStore<IdentityRole>(context);
            //    var manager = new RoleManager<IdentityRole>(store);
            //    var role = new IdentityRole { Name = "User" };

            //    manager.Create(role);
            //}

            ////add User as admin role
            //if (!context.Users.Any(r => r.UserName == "admin"))
            //{
            //    var store = new UserStore<ApplicationUser>(context);
            //    var manager = new UserManager<ApplicationUser>(store);
            //    var user = new ApplicationUser { UserName = "admin" };

            //    manager.Create(user, "123456");
            //    manager.AddToRole(user.Id, "Admin");
            //}

            ////add Project information 
            //var lsProjects = GetJsonData<Project>(Project_Url);
            //var listProjects = lsProjects.Select(f => new Project() 
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        ApplicationUserId = f.ApplicationUserId,
            //        Name = f.Name,
            //        Description = f.Description,
            //        Goal = f.Goal,
            //        CurrentFund = f.CurrentFund,
            //        StartDate = f.StartDate,
            //        ExpireDate = f.ExpireDate,
            //        Status = f.Status,
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now,
            //        DeletedAt = null,
            //    })
            //    .ToList();

            //context.Projects.AddRange(listProjects);
            //context.SaveChanges();

            ////add Image information 
            //var lsImages = GetJsonData<ProjectImage>(Image_Url);
            //var listImages = lsImages.Select(f => new ProjectImage()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        ProjectId = f.ProjectId,
            //        Url = f.Url,
            //        Description = f.Description,
            //        Status = f.Status,
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now,
            //        DeletedAt = null,
            //    })
            //    .ToList();

            //context.Projects.AddRange(listProjects);
            //context.SaveChanges();

            ////Blog
            ////ProjectComment
            ////Donation
            ////Follow

        }
        ////Get Json Data
        //private List<T> GetJsonData<T>(string url) where T : new()
        //{
        //    var client = new HttpClient();
        //    var responseContent = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        //    var list = JsonConvert.DeserializeObject<List<T>>(responseContent);
        //    return list;
        //}
    }
}
