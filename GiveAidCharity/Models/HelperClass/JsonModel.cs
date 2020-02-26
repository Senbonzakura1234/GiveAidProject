using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiveAidCharity.Models
{
    public class projectJsonModel
    {
        public string name { get; set; }
        public double currentFund { get; set; }
        public DateTime startDate { get; set; }
    }
}