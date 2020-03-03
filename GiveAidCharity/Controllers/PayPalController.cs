using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GiveAidCharity.Models;
using GiveAidCharity.Models.HelperClass;
using GiveAidCharity.Models.Main;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GiveAidCharity.Controllers
{
    public class PayPalController : Controller
    {
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();
        private const string BusinessEmail = "sb-gboj43907912@business.example.com"; 
        private const string Currency = "USD"; 
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;

        public PayPalController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        public PayPalController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        [HttpPost]
        public async Task<HttpStatusCodeResult> Receive()
        {
            //Store the IPN received from PayPal
            var listString = LogRequest(Request);
            var input = listString.Aggregate("", (current, item) => current + (item + " = " + Request[item] + "; "));
            Debug.WriteLine(input);

            if (Request["business"] == null || Request["txn_id"] == null || 
                Request["custom"] == null || Request["item_number"] == null)
            {
                return null;
            }

            if (Request["business"] != BusinessEmail) return null;
            if (Request["mc_currency"] != Currency) return null;

            if (!CheckValidAmount(Request["payment_gross"])) return null;
            var amount = double.Parse(Request["payment_gross"]);

            var user = await UserManager.FindByIdAsync(Request["custom"]);
            var checkUser = user != null && user.Status == ApplicationUser.UserStatusEnum.Activated;
            if (!checkUser) return null;

            var donation = new Donation
            {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                PaymentMethod = Donation.PaymentMethodEnum.Paypal,
                ApplicationUserId = user.Id,
                ProjectId = Request["item_number"],
                txn_id = Request["txn_id"],
                business = Request["business"],
                receiver_id = Request["receiver_id"],
                payer_id = Request["payer_id"],
                payer_email = Request["payer_email"],
                payment_gross = Request["payment_gross"],
                mc_gross = Request["mc_gross"],
                extra_info = input
            };

            //Fire and forget verification task
            var result = await Task.Run(async () => await VerifyTask(Request, donation));
            return result ? new HttpStatusCodeResult(HttpStatusCode.OK) : null;
            //Reply back a 200 code
        }
        // ReSharper disable once UnusedParameter.Local
        private static IEnumerable<string> LogRequest(HttpRequestBase request)
        {
            return (from object item in request.Params select item.ToString()).ToList();
        }
        private async Task<bool> VerifyTask(HttpRequestBase ipnRequest, Donation donation)
        {
            Debug.WriteLine(ipnRequest);
            var verificationResponse = string.Empty;
            try
            {
                var verificationRequest = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");

                //Set values for the verification request
                verificationRequest.Method = "POST";
                verificationRequest.ContentType = "application/x-www-form-urlencoded";
                var param = Request.BinaryRead(ipnRequest.ContentLength);
                var strRequest = Encoding.ASCII.GetString(param);

                //Add cmd=_notify-validate to the payload
                strRequest = "cmd=_notify-validate&" + strRequest;
                verificationRequest.ContentLength = strRequest.Length;

                //Attach payload to the verification request
                var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII);
                await streamOut.WriteAsync(strRequest);
                streamOut.Close();

                //Send the request to PayPal and get the response
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream() 
                                                ?? throw new InvalidOperationException());
                verificationResponse = await streamIn.ReadToEndAsync();
                streamIn.Close();
            }
            #pragma warning disable 168
            catch (Exception exception)
            #pragma warning restore 168
            {
                //Capture exception for manual investigation
            }
            return await ProcessVerificationResponse(verificationResponse, donation);
        }
        private static async Task<bool> ProcessVerificationResponse(string verificationResponse, Donation donation)
        {
            var project = await Db.Projects.FindAsync(donation.ProjectId);
            if (project == null) return false;
            var checkDonation = await Db.Donations.Where(d => d.txn_id == donation.txn_id).FirstOrDefaultAsync();
            if (checkDonation != null) return false;
            if (verificationResponse.Equals("VERIFIED"))
            {
                donation.Status = Donation.DonationStatusEnum.Success;
                donation.CreatedAt = DateTime.Now;
                donation.UpdatedAt = DateTime.Now;
                project.CurrentFund += donation.Amount;
                Db.Entry(project).State = EntityState.Modified;
            }
            else
            {
                donation.Status = Donation.DonationStatusEnum.Fail;
                donation.CreatedAt = DateTime.Now;
                donation.UpdatedAt = DateTime.Now;
            }
            var result = donation.Status == Donation.DonationStatusEnum.Success;
            Db.Donations.Add(donation);
            await Db.SaveChangesAsync();
            HelperMethod.NotifyEmailTransaction("Transaction no " + donation.Id + " is" +
                                                donation.Status, "Transaction Result" , donation);
            return result;
            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment
        }
        private static bool CheckValidAmount(string amount)
        {
            return double.TryParse(amount, out var output) && !double.IsNaN(output) && !double.IsInfinity(output);
        }
    }
}