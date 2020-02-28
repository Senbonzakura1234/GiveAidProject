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
        //This is similar to Reddit Post rating system, there are 2 buttons UpVote and DownVote.
        //click UpVote to UpVote and click DownVote to DownVote.
        //when redo the vote, it means the vote status is neutral.
        //Query to DB to get all vote of a blog
        //+1 if UpVote, -1 if DownVote, +0 if Neutral.
        //when an user 1st time UpVote or DownVote a Blog, create a new Vote record in DB with the status that the user want.
        //when an user redo their UpVote or DownVote action, find all of their vote of that Blog in DB, set status of each to Neutral.
        //when an user change their vote of of a post, do the same when they redo their vote,
        //then create a new Vote record in DB with the status that the user want.
        //using ajax for UpVoting and DownVoting by users and recount it after user's votes

        public string Rss { get; set; }
        //this contain Id of project that directly relate to the blog,
        //use this to create link to redirect to the cause detail page of the project


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