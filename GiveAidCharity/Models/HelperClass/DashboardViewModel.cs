using GiveAidCharity.Models.Main;
using System;
using System.Collections.Generic;
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
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        public Donation.DonationStatusEnum Status { get; set; }
    }
}