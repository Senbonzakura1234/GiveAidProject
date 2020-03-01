using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GiveAidCharity.Models.Main;

namespace GiveAidCharity.Models.HelperClass
{
    public class HelperMethod
    {
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();

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

        internal static async Task<int> RatingCount(string id)
        {
            if (id == null) return -(int) HttpStatusCode.BadRequest;
            var blog = await Db.Blogs.FindAsync(id);
            if (blog == null) return -(int)HttpStatusCode.NotFound;
            var votes = await Db.Votes.Where(d => d.Status != Vote.VoteStatusEnum.Neutral && d.BlogId == id).ToListAsync();
            return votes.Sum(item => (int) item.Status);
        } 
    }
}