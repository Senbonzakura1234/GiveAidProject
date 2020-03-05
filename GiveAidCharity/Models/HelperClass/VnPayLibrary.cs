using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GiveAidCharity.Models.HelperClass
{
    internal class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public string GetExtraData()
        {
            return _responseData.Aggregate("", (current, item) => current + (item.Key + " = " + item.Value));
        }
        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        #region Request

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(kv.Key + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
            }
            var queryString = data.ToString();
            var rawData = GetRequestRaw();
            baseUrl += "?" + queryString;
            var vnpSecureHash = Utils.Sha256(vnpHashSecret + rawData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;
            return baseUrl;
        }

        private string GetRequestRaw()
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(kv.Key + "=" + kv.Value + "&");
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion

        #region Response process

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseRaw();
            var myChecksum = Utils.Sha256(secretKey + rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseRaw()
        {

            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (var kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(kv.Key + "=" + kv.Value + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion
    }

    public class Utils
    {
        // ReSharper disable once UnusedMember.Global
        public static string Md5(string sInput)
        {
            // ReSharper disable once RedundantAssignment
            var algorithmType = default(HashAlgorithm);
            var enCoder = new ASCIIEncoding();
            var valueByteArr = enCoder.GetBytes(sInput);
            // ReSharper disable once RedundantAssignment
            byte[] hashArray = null;
            // Encrypt Input string 
            algorithmType = new MD5CryptoServiceProvider();
            hashArray = algorithmType.ComputeHash(valueByteArr);
            //Convert byte hash to HEX
            var sb = new StringBuilder();
            foreach (var b in hashArray)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
        public static string Sha256(string data)
        {
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                var builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}