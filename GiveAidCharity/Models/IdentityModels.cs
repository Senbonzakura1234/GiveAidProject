using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using GiveAidCharity.Models.Main;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GiveAidCharity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ProjectComment> ProjectComments { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Url]
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string CompanyName { get; set; }
        public GenderEnum Gender { get; set; }
        public enum GenderEnum
        {
            [Display(Name = "Unknown")]
            Unknown = 0,
            [Display(Name = "Female")]
            Female = 1,
            [Display(Name = "Male")]
            Male = 2,
            [Display(Name = "Others")]
            Others = 3,
            [Display(Name = "Private")]
            Private = -1,

        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthday { get; set; }

        public UserStatusEnum Status { get; set; }

        public enum UserStatusEnum
        {
            Inactivated = 0,
            Activated = 1,
            Locked = 2,
            Banned = 3
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = UserStatusEnum.Activated;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationUserRole : IdentityRole
    {
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public ApplicationUserRole()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Category> Categories { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }
    }
}