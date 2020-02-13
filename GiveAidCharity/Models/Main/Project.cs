using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAidCharity.Models.Main
{
    public class Project
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<ProjectImage> ProjectImages { get; set; }
        public virtual ICollection<ProjectComment> ProjectComments { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Url]
        [Required]
        public string CoverImg { get; set; }
        [Required]
        public string ContentPart1 { get; set; }
        [Required]
        public string ContentPart2 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Goal { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double CurrentFund { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpireDate { get; set; }

        public ProjectStatusEnum Status { get; set; }
        public enum ProjectStatusEnum
        {
            Establishing = 0,
            Pending = 1,
            Ongoing = 2,
            Success = 3,
            Suspended = -1,
            Canceled = -2
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

        public Project()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            CurrentFund = 0;
            Status = ProjectStatusEnum.Establishing;
        }
    }
}