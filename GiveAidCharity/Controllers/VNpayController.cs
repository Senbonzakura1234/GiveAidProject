using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace GiveAidCharity.Controllers
{
    [Authorize]
    public class VNpayController : Controller
    {
        // GET: VNpay

        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private const string ExchangeRateUsdApi = "https://free.currconv.com/api/v7/convert?q=USD_VND&compact=ultra&apiKey=549fdf0ffb9b928a00a6";

        public VNpayController()
        {
        }

        public VNpayController(
            ApplicationUserManager userManager
        )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }


        [HttpPost]
        public async Task<ActionResult> Donate(string userId, string projectId, double? amountVnpay)
        {
            if (amountVnpay == null || amountVnpay < 1)
            {
                return RedirectToAction("CauseDetail", "Home", new {id = projectId});
            }
            if (userId == null || projectId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await UserManager.FindByIdAsync(userId);
            var project = await _db.Projects.FindAsync(projectId);
            if (user == null || project == null) return new HttpNotFoundResult();


            var donation = new Donation
            {
                ProjectId = projectId,
                ApplicationUserId = userId,
                Amount = amountVnpay.Value,
                PaymentMethod = Donation.PaymentMethodEnum.VnPay,
            };
            _db.Donations.Add(donation);
            await _db.SaveChangesAsync();

            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", "2.0.0");
            vnPay.AddRequestData("vnp_Command", "donate");
            vnPay.AddRequestData("vnp_TmnCode", "F0NO7BAG");
            vnPay.AddRequestData("vnp_Amount", (donation.Amount * 100 * GetExchangeRate()).ToString(CultureInfo.InvariantCulture));
            vnPay.AddRequestData("vnp_BankCode", "NCB");
            vnPay.AddRequestData("vnp_BankTranNo", "9704198526191432198");
            vnPay.AddRequestData("vnp_CardType", "ATM");
            vnPay.AddRequestData("vnp_CreateDate", donation.CreatedAt.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", "Order test");
            vnPay.AddRequestData("vnp_OrderType", "other");
            if (Request.Url != null)
                vnPay.AddRequestData("vnp_ReturnUrl", Url.Action("Ipn", "VNpay", null, Request.Url.Scheme));
            vnPay.AddRequestData("vnp_TxnRef", donation.Id);
            var paymentUrl = vnPay.CreateRequestUrl("http://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "OWESKVDWDQHLTYAIZCVYVSNSAOBITQDX");

        

            return Redirect(paymentUrl);

        }
        public async Task<ActionResult> Ipn()
        {
            const string vnpHashSecret = "OWESKVDWDQHLTYAIZCVYVSNSAOBITQDX"; //Secret key
            var vnPayData = Request.QueryString;
            var vnPay = new VnPayLibrary();


            foreach (string s in vnPayData)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(s, vnPayData[s]);
                }
            }


            var orderId = vnPay.GetResponseData("vnp_TxnRef");
            // ReSharper disable once UnusedVariable
            var vnPayTranId = vnPay.GetResponseData("vnp_TransactionNo");
            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = Request.QueryString["vnp_SecureHash"];
            var checkSignature = vnPay.ValidateSignature(vnpSecureHash, vnpHashSecret);
            if (!checkSignature) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var checkDonation = (await _db.Donations.Where(d => d.vnp_TransactionNo == vnPayTranId)
                    .OrderByDescending(d => d.CreatedAt).ToListAsync()).Count == 0;
            
            var donation = await _db.Donations.FindAsync(orderId);
            if (donation == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var project = await _db.Projects.FindAsync(donation.ProjectId);
            if (project == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            donation.extra_info = vnPay.GetExtraData();
            donation.vnp_TransactionNo = vnPayTranId;
            donation.vnp_ResponseCode = vnpResponseCode;

            if (checkDonation)
            {
                if (donation.Status == Donation.DonationStatusEnum.Pending)
                {
                    if (vnpResponseCode == "00")
                    {
                        donation.Status = Donation.DonationStatusEnum.Success;
                        project.CurrentFund += donation.Amount;
                        _db.Entry(project).State = EntityState.Modified;
                    }
                    else
                    {
                        donation.Status = Donation.DonationStatusEnum.Cancel;
                    }
                }
            }
            else
            {
                donation.Status = Donation.DonationStatusEnum.Cancel;
            }
            donation.UpdatedAt = HelperMethod.GetCurrentDateTimeWithTimeZone(DateTime.UtcNow);


            _db.Entry(donation).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            HelperMethod.NotifyEmailTransaction("Transaction no " + donation.Id + " is" +
                                                donation.Status, "Transaction Result", donation,
                "Transaction Result", "anhdungpham090@gmail.com");
            return RedirectToAction("VnPayResult", "Payment", new { id = orderId });
        }


        private static double GetExchangeRate()
        {
            var getPriceVnd = new HttpClient();
            double usdVnd;
            try
            {
                var responseContent = getPriceVnd.GetAsync(ExchangeRateUsdApi).Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var exchangeRate = JsonConvert.DeserializeObject<ObservableCollection<CurrencyConvertModel>>(responseContent);
                usdVnd = exchangeRate?.FirstOrDefault()?.USD_VND ?? 23238.5;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                usdVnd = 23238.5;
            }
            return usdVnd;
        }
        
    }
}