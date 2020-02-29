using System;

namespace GiveAidCharity.Models.HelperClass
{
    public class HelperMethod
    {
        public static bool CheckValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }

        public static string SubString(string text)
        {
            if (text.Length > 5)
            {
                return text.Substring(5) + "...";
            }

            return text;
        }
    }
}