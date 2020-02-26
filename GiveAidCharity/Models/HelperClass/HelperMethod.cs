using System;

namespace GiveAidCharity.Models.HelperClass
{
    public class HelperMethod
    {
        public static bool CheckValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}