using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAidCharity.Models.Main
{
    public class Blog
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        //Url to project
        public string Rss { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string ContentPart1 { get; set; }
        [Required]
        public string ContentPart2 { get; set; }
        public string ContentPart3 { get; set; }
        
        public BlogStatusEnum Status { get; set; }
        public enum BlogStatusEnum
        {
            Pending = 0,
            Published = 1,
            UnPublished = 2,
            Deleted = -1
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

        public Blog()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = BlogStatusEnum.Pending;
        }
    }
}