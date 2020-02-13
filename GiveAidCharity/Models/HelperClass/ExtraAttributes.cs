using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiveAidCharity.Models.HelperClass
{
    public class ProjectStartDateAttribute : RangeAttribute
    {
        public ProjectStartDateAttribute()
            : base(typeof(DateTime),
                DateTime.Now.AddDays(10).ToShortDateString(), DateTime.Now.AddDays(50).ToShortDateString())
        { }
    }
    public class ProjectEndDateAttribute : RangeAttribute
    {
        public ProjectEndDateAttribute()
            : base(typeof(DateTime),
                DateTime.Now.AddDays(20).ToShortDateString(), DateTime.Now.AddDays(60).ToShortDateString())
        { }
    }
}