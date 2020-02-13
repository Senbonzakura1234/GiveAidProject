using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAidCharity.Models.Main
{
    public class ProjectComment
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("Project")]
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string ParentId { get; set; }

        [Required]
        public string Content { get; set; }

        public ProjectCommentStatusEnum Status { get; set; }
        public enum ProjectCommentStatusEnum
        {
            Published,
            UnPublished,
            Deleted
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

        public ProjectComment()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = ProjectCommentStatusEnum.Published;
        }
    }
}