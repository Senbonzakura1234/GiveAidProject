using GiveAidCharity.Models.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiveAidCharity.Models.HelperClass
{
    public class ProjectListViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CoverImg { get; set; }
        public double Goal { get; set; }
        public double CurrentFund { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpireDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        public Project.ProjectStatusEnum Status { get; set; }
    }
    public class DonationListViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Avatar { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public double Amount { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        public Donation.DonationStatusEnum Status { get; set; }
        public Donation.PaymentMethodEnum PaymentMethod { get; set; }
    }

    public class ProjectCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(100, double.MaxValue, ErrorMessage = "Goal must be at least 100$")]
        public double Goal { get; set; }

        [Url]
        [Required]
        public string CoverImg { get; set; }
        [Required]
        public string ContentPart1 { get; set; }
        [Required]
        public string ContentPart2 { get; set; }

        //[ProjectStartDate(ErrorMessage = "Project can't be started sooner then 10 days from today")]
        [DisplayName("Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        //[ProjectEndDate(ErrorMessage = "Project period can't be less than 10 days")]
        [DisplayName("End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpireDate { get; set; }

        public string CategoryId { get; set; }
        public string ListImg { get; set; }

        [Required(ErrorMessage = "Your business Email is required")]
        [EmailAddress]
        public string ReceiverEmail { get; set; }

        public ProjectCreateViewModel()
        {
            Goal = 100;
            CoverImg = "https://res.cloudinary.com/senbonzakura/image/upload/v1581505882/Give%20Aid%20Project/2647372207_824b08c6-bf87-4cf4-afbf-5c54273b18e1_votmhh.png";
            StartDate = DateTime.Now.AddDays(11);
            ExpireDate = DateTime.Now.AddDays(21);
        }
    }

    public class EditProjectStatusViewModel
    {
        public string Id { get; set; }
        public Project.ProjectStatusEnum Status { get; set; }
    }
}