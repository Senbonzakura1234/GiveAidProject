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
using GiveAidCharity.Models.Main;

namespace GiveAidCharity.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class IPNPayPalController : Controller
    {
        private static int _ipnRequestCount;
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ipn()
        {
            _ipnRequestCount++;
            ViewBag.Message = "Count ipn request. " + _ipnRequestCount;
            return View("Index");
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Check out success.";
            return View("Index");
        }
        [HttpPost]
        public async Task<HttpStatusCodeResult> Receive()
        {
            //Store the IPN received from PayPal
            var listString = LogRequest(Request);
            var input = listString.Aggregate("", (current, item) => current + (item + " = " + Request[item] + "; "));

            var donation = new Donation
            {

            };
            Db.Donations.Add(donation);
            await Db.SaveChangesAsync();
            //Fire and forget verification task
            await Task.Run(() => VerifyTask(Request));
            

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private async Task VerifyTask(HttpRequestBase ipnRequest)
        {
            Debug.WriteLine(ipnRequest);
            var verificationResponse = string.Empty;
            var txnId = Request["txn_id"];
            var projectId = Request["item_number"];
            if (txnId == null) return;
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
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream() ?? throw new InvalidOperationException());
                verificationResponse = await streamIn.ReadToEndAsync();
                streamIn.Close();
            }
            #pragma warning disable 168
            catch (Exception exception)
            #pragma warning restore 168

            {
                //Capture exception for manual investigation
            }
            await ProcessVerificationResponse(verificationResponse, txnId, projectId);
        }


        // ReSharper disable once UnusedParameter.Local
        private static IEnumerable<string> LogRequest(HttpRequestBase request)
        {
            return (from object item in request.Params select item.ToString()).ToList();
        }

        private static async Task ProcessVerificationResponse(string verificationResponse, string txnId, string projectId)
        {
            var donations = await Db.Donations.Where(p => p.Id == txnId)
                .OrderByDescending(p => p.CreatedAt).ToListAsync(); // replace p.Id with p.txn_id
            var donation = donations.FirstOrDefault();
            if (donation == null) return;
            if (verificationResponse.Equals("VERIFIED"))
            {
                donation.Status = Donation.DonationStatusEnum.Success;
                var project = await Db.Projects.FindAsync(projectId);
                if (project == null) return;
                project.CurrentFund += donation.Amount;
                Db.Entry(donation).State = EntityState.Modified;
                Db.Entry(project).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return;
            }

            if (verificationResponse.Equals("INVALID"))
            {
                donation.Status = Donation.DonationStatusEnum.Fail;
                Db.Entry(donation).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return;
            }

            donation.Status = Donation.DonationStatusEnum.Fail;
            Db.Entry(donation).State = EntityState.Modified;
            await Db.SaveChangesAsync();

            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment
        }
    }

}