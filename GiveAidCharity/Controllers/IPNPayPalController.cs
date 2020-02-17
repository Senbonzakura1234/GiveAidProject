using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GiveAidCharity.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class IPNPayPalController : Controller
    {
        private static int _ipnRequestCount;
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
#pragma warning disable 168
            catch (Exception exception)
#pragma warning restore 168
            {
                //Capture exception for manual investigation
            }

            ProcessVerificationResponse(verificationResponse);
        }


        // ReSharper disable once UnusedParameter.Local
        private static void LogRequest(HttpRequestBase request)
        {
            // Persist the request values into a database or temporary data store
        }

        private static void ProcessVerificationResponse(string verificationResponse)
        {
            if (verificationResponse.Equals("VERIFIED"))
            {
                // check that Payment_status=Completed
                // check that Txn_id has not been previously processed
                // check that Receiver_email is your Primary PayPal email
                // check that Payment_amount/Payment_currency are correct
                // process payment
            }
            else if (verificationResponse.Equals("INVALID"))
            {
                //Log for manual investigation
            }
            else
            {
                //Log error
            }
        }
    }

}