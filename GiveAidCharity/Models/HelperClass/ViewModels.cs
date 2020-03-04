using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GiveAidCharity.Models.Main;

namespace GiveAidCharity.Models.HelperClass
{
    public class CausesListViewModel
    {
        public string Id { get; set; }
        public string CoverImg { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Goal { get; set; }
        public double CurrentFund { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpireDate { get; set; }
        public int FollowCount { get; set; }
    }

    public class HomeViewModel
    {
        public IEnumerable<CausesListViewModel> CausesList { get; set; }
        public IEnumerable<DonationsListViewModel> DonationList { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }

    public class CausesDetailViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string HostName { get; set; }
        public string HostAvatar { get; set; }
        public string HostDescription { get; set; }
        public string HostEmail { get; set; }
        public string HostPhone { get; set; }
        public string Description { get; set; }
        public double Goal { get; set; }
        public string CoverImg { get; set; }
        public double CurrentFund { get; set; }
        [Required]
        public string ContentPart1 { get; set; }
        [Required]
        public string ContentPart2 { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpireDate { get; set; }
        public Project.ProjectStatusEnum Status { get; set; }
        public int FollowCount { get; set; }
        public virtual List<ProjectImage> ProjectImages { get; set; }
        public virtual List<ProjectComment> ProjectComments { get; set; }
        public Follow.FollowStatusEnum CurrentUserFollowStatus { get; set; }
    }
    public class SingleDonationViewModel {
        public string UserId { get; set; }
        public string Username { get; set; }
        public double Amount { get; set; }
        public string CauseId { get; set; }
        public string Avatar { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DonateDate { get; set; }
    }
    public class DonationsGroupByUserViewModel
    {
        public string UserId { get; set; }
        public virtual List<SingleDonationViewModel> DonationGroups { get; set; }
        public double TotalGroupAmount { get; set; }
    }

    public class DonationsListViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DonateDate { get; set; }
        public double Amount { get; set; }
        public CausesListViewModel Cause { get; set; }
    }
    // ReSharper disable  InconsistentNaming
    public class TransactionResult
    {
        [Display(Name = "Transaction No")]
        public string Id { get; set; }

        [Display(Name = "VNpay Transaction No")]
        public string vnp_TransactionNo { get; set; }

        [Display(Name = "VnPay Transaction No")]
        public string txn_id { get; set; }

        [Display(Name = "Paypal Id")]
        public string payer_id { get; set; }
        [Display(Name = "Paypal Email")]
        public string payer_email { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DonateDate { get; set; }

        [Display(Name = "Amount")]
        public double Amount { get; set; }
        public string ProjectId { get; set; }
        [Display(Name = "Project")]
        public string ProjectName { get; set; }
        [Display(Name = "Status")]
        public Donation.DonationStatusEnum Status { get; set; }
        [Display(Name = "Payment Method")]
        public Donation.PaymentMethodEnum PaymentMethod { get; set; }
    }

    //Use this to create Blog list view, remember to filter to show blog has Published as status
    public class BlogListViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; } // avatar of user who create this post
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Vote { get; set; }
        public int Comment { get; set; }
        //this field showing Vote points of this post
        public string Title { get; set; }
        public string Description { get; set; } // This == ContentPart1 of Blog
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
    }

    //Use this to create Blog detail view
    public class BlogDetailViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; } // avatar of user who create this post
        public Category Category { get; set; }
        public int Vote { get; set; }
        //this field showing Vote points of this post
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public string Title { get; set; }
        public string ContentPart1 { get; set; }
        public string ContentPart2 { get; set; }
        public string ContentPart3 { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        public Vote.VoteStatusEnum CurrentUserVoteStatus { get; set; }
    }
    public class CategoryViewModel{
        public string Id { get; set; }
        public string Name { get; set; }
        public int ProjectCount { get; set; }
    }
}