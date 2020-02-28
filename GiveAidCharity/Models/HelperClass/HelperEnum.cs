using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiveAidCharity.Models.HelperClass
{
    public class HelperEnum
    {
        public enum DonationSortEnum
        {
            CreatedAt = 0,
            ProjectName = 1,
            Amount = 2,
            PaymentMethod = 3,
            Status = 4,
            UserName = 5
        }

        public enum DonationDirectEnum
        {
            Asc = 0,
            Desc = 1
        }

        public enum ProjectSortEnum
        {
            StartDate = 0,
            ExpireDate = 1,
            ProjectName = 2,
            ProcessPercent = 3,
            Status = 4
        }

        public enum ProjectDirectEnum
        {
            Asc = 0,
            Desc = 1
        }
    }
}