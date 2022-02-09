using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
/// <summary>
/// 与 Http 相关的操作
/// </summary>
namespace SensorsData.ABTest
{
    public class HttpManager
    {
        private readonly string serverUrl;
        private readonly string JSON_MIMETYPE = "application/json";

        public HttpManager(string serverUrl)
        {
            this.serverUrl = serverUrl;
        }

        public string SendToServer(string content, int requestTimeoutMillisecond)
        {
            //string encodedContent = System.Web.HttpUtility.UrlEncode(content);

            var request = (HttpWebRequest)WebRequest.Create(serverUrl);
            request.Method = "POST";
            request.ReadWriteTimeout = requestTimeoutMillisecond;
            request.Timeout = requestTimeoutMillisecond;
            request.UserAgent = "SensorsAnalytics DotNET SDK";
            //request.Headers["Content-Type"] = JSON_MIMETYPE;
            request.ContentType = JSON_MIMETYPE;

            var data = Encoding.ASCII.GetBytes(content);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new SystemException("Sensors Analytics SDK send response is not 200, content: " + responseString);
            }

            return responseString;
        }

        private string GzipAndBase64(string inputStr)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                var outputBytes = outputStream.ToArray();
                var base64Output = Convert.ToBase64String(outputBytes);
                return base64Output;
            }
        }
    }
}
