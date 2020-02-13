using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiveAidCharity.Models.HelperClass
{
    public class ProjectCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(100, double.MaxValue, ErrorMessage = "Goal must be at least 100$")]
        public double Goal { get; set; }

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
    }
}