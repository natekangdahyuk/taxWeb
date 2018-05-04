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
using System.Net.Http;

namespace Common.Lib
{
    public class Networks
    {        
        //웹 페이지 post 로 보내기
        public static string sendPost(string RestFulAddress, string strData)
        {
            
            WebRequest request = WebRequest.Create(RestFulAddress);//환경에 따라 주소가 바뀌면 된다.


            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            //string postData = "WG_Protocol=" + strData; //암호화로 랩핑할 경우
            string postData = strData; //그냥 json 을 던질 경우


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

        //클라이언트 IP 확인하기
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }

    //암호화
    public class NetWokrBase
    {
        private static string m_szIv = "SexyCome";
        private static string m_key = "thankyousomuchIamyourmom";

        public static string Encrypt(string input)
        {
            TripleDES tripleDes = TripleDES.Create();
            tripleDes.IV = Encoding.ASCII.GetBytes(m_szIv);
            tripleDes.Key = Encoding.ASCII.GetBytes(m_key);
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.Zeros;

            ICryptoTransform crypto = tripleDes.CreateEncryptor();
            byte[] decodedInput = Encoding.UTF8.GetBytes(input);
            byte[] decryptedBytes = crypto.TransformFinalBlock(decodedInput, 0, decodedInput.Length);
            return Encoder(decryptedBytes);
        }

        public static string Encoder(byte[] input)
        {
            string ret = "";
            ret = BitConverter.ToString(input).Replace("-", string.Empty).ToLower();
            return ret;
        }

        public static string Decrypt(string input)
        {
            TripleDES tripleDes = TripleDES.Create();
            tripleDes.IV = Encoding.ASCII.GetBytes(m_szIv);
            tripleDes.Key = Encoding.ASCII.GetBytes(m_key);
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.Zeros;

            ICryptoTransform crypto = tripleDes.CreateDecryptor();
            byte[] decodedInput = Decoder(input);
            byte[] decryptedBytes = crypto.TransformFinalBlock(decodedInput, 0, decodedInput.Length);

            return Encoding.ASCII.GetString(decryptedBytes);
            //           return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static byte[] Decoder(string input)
        {
            byte[] bytes = new byte[input.Length / 2];
            int targetPosition = 0;

            for (int sourcePosition = 0; sourcePosition < input.Length; sourcePosition += 2)
            {
                string hexCode = input.Substring(sourcePosition, 2);
                bytes[targetPosition++] = Byte.Parse(hexCode, NumberStyles.AllowHexSpecifier);
            }

            return bytes;
        }
    }

}
