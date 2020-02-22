using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public PaymentController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        public PaymentController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        // ReSharper disable once InconsistentNaming
        public async Task<ActionResult> Result(string tx)
        {
            if (tx == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var donation = await _db.Donations.Where(d => d.txn_id == tx)
                .OrderByDescending(d => d.CreatedAt).FirstOrDefaultAsync();
            if (donation?.ApplicationUserId == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var projectName = (await _db.Projects.FindAsync(donation.ProjectId))?.Name;

            var user = await UserManager.FindByIdAsync(donation.ApplicationUserId);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            if (user.Id != CurrentUserId) return RedirectToAction("Index", "Home");
            var transactionResult = new TransactionResult
            {
                Username = user.UserName,
                Amount = donation.Amount,
                Id = donation.Id,
                ProjectId = donation.ProjectId,
                txn_id = donation.txn_id,
                DonateDate = donation.CreatedAt,
                UserId = user.Id,
                ProjectName = projectName,
                payer_email = donation.payer_email,
                payer_id = donation.payer_id,
                vnp_TransactionNo = donation.vnp_TransactionNo,
                Status = donation.Status,
                PaymentMethod = donation.PaymentMethod
            };
            return View(transactionResult);
        }
    }
}