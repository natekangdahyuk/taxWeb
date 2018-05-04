using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.IO;
using System.Net;
using System.Globalization;
using System.Security.Cryptography;
using System.Web;
using System.Collections.Specialized;

namespace codTaxWeb.WebProtocols
{
    public partial class EXtAPI
    {
        //dict -> string
        public static string DictToString(Dictionary<string, string> dict)
        {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in dict)
            {
                builder.Append(kvp.Key + "=" + kvp.Value + "&");
            }

            return builder.ToString();
        }

        //Web API POST 로 호출
        public static string SendPostData(string url, string postData)
        {
            WebRequest request = WebRequest.Create(url);

            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            //string postData = "WG_Protocol=" + strData; //암호화로 랩핑할 경우

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            string response_Status = ((HttpWebResponse)response).StatusDescription; //디버깅용
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();// 디버깅용            

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
    }
}