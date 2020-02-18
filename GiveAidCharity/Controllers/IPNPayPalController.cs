using System;
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

namespace GiveAidCharity.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class IPNPayPalController : Controller
    {
        private static int _ipnRequestCount;
        private static readonly ApplicationDbContext dbContext = new ApplicationDbContext();
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
        public HttpStatusCodeResult Receive()
        {
            //Store the IPN received from PayPal
            LogRequest(Request);

            //Fire and forget verification task
            Task.Run(() => VerifyTask(Request));

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void VerifyTask(HttpRequestBase ipnRequest)
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
                streamOut.Write(strRequest);
                streamOut.Close();

                //Send the request to PayPal and get the response
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream() ?? throw new InvalidOperationException());
                verificationResponse = streamIn.ReadToEnd();
                streamIn.Close();

            }

            catch (Exception exception)

            {
                //Capture exception for manual investigation
            }

            ProcessVerificationResponse(verificationResponse);
        }


        // ReSharper disable once UnusedParameter.Local
        private static void LogRequest(HttpRequestBase request)
        {
           
        }

        private static void ProcessVerificationResponse(string verificationResponse)
        {
            if (verificationResponse.Equals("VERIFIED"))
            {
                Debug.WriteLine("okay");
                var project = dbContext.Projects.Find("02b751ab-548c-4695-8f3a-44e87be832ef");
                project.Description = "ok";
                dbContext.Entry(project).State = EntityState.Modified;
                dbContext.SaveChanges();
                // check that Payment_status=Completed
                // check that Txn_id has not been previously processed
                // check that Receiver_email is your Primary PayPal email
                // check that Payment_amount/Payment_currency are correct
                // process payment
            }
            else if (verificationResponse.Equals("INVALID"))
            {
                var project = dbContext.Projects.Find("02b751ab-548c-4695-8f3a-44e87be832ef");
                project.Description = "not ok";
                dbContext.Entry(project).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            else
            {
                var project = dbContext.Projects.Find("02b751ab-548c-4695-8f3a-44e87be832ef");
                project.Description = "error";
                dbContext.Entry(project).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }
    }

}