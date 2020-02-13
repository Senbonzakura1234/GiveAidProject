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

        [Url]
        [Required]
        public string CoverImg { get; set; }
        [Required]
        public string Content { get; set; }

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

        public ProjectCreateViewModel()
        {
            Goal = 100;
            CoverImg = "https://res.cloudinary.com/senbonzakura/image/upload/v1581505882/Give%20Aid%20Project/2647372207_824b08c6-bf87-4cf4-afbf-5c54273b18e1_votmhh.png";
            StartDate = DateTime.Now.AddDays(11);
            ExpireDate = DateTime.Now.AddDays(21);
        }
    }

    public class CausesListViewModel
    {
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
    }
}