using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FastX.Core.Helpers
{
    public class HttpHelper
    {
        public sealed class ping
        {
            private const int INTERNET_CONNECTION_LAN = 2;
            private const int INTERNET_CONNECTION_MODEM = 1;

            public static bool CheckServeStatus()
            {
                if (!LocalConnectionStatus())
                {
                    Console.WriteLine("网络异常~无连接");
                    return false;
                }
                Console.WriteLine("网络正常");
                return true;
            }

            [DllImport("winInet.dll")]
            private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);
            private static bool LocalConnectionStatus()
            {
                int dwFlag = 0;
                if (!InternetGetConnectedState(ref dwFlag, 0))
                {
                    Console.WriteLine("LocalConnectionStatus--未连网!");
                    return false;
                }
                if ((dwFlag & 1) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用调制解调器上网。");
                    return true;
                }
                if ((dwFlag & 2) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用网卡上网。");
                    return true;
                }
                return false;
            }

            public static bool MyPing(string[] urls, out int errorCount)
            {
                bool flag = true;
                Ping ping = new Ping();
                errorCount = 0;
                try
                {
                    for (int i = 0; i < urls.Length; i++)
                    {
                        PingReply reply = ping.Send(urls[i]);
                        if (reply.Status != IPStatus.Success)
                        {
                            flag = false;
                            errorCount++;
                        }
                        Console.WriteLine("Ping " + urls[i] + "    " + reply.Status.ToString());
                    }
                }
                catch
                {
                    flag = false;
                    errorCount = urls.Length;
                }
                return flag;
            }
        }
        internal HttpWebResponse CreateGetHttpResponse(string url)
        {
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
            request1.Timeout = 6000;
            request1.ContentType = "text/html;chartset=UTF-8";
            request1.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 48.0) Gecko / 20100101 Firefox / 48.0";
            request1.Method = "GET";
            return (HttpWebResponse)request1.GetResponse();
        }
        internal string Get(string URL, string Type)
        {
            string result = "";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            var header = new WebHeaderCollection();
            header.Add(Type);
            req.Headers = header;

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        public string getHtml(string url)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            Stream stream;
            WebClient client = new WebClient();
            try
            {
                stream = client.OpenRead(url);
                stream.ReadTimeout = 1000;
            }
            catch (WebException exception1)
            {
                string message = exception1.Message;
                return null;
            }
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            try
            {
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }
        internal string getHtml(string url, bool WL)
        {
            if (WL == true)
            {
                if (!ping.CheckServeStatus())
                {
                    return null;
                }

            }
            return getHtml(url);
        }
        public string Post(string URL, string jsonParas)
        {
            return Post(URL, jsonParas, "application/json");
        }
        public string Post(string URL, string jsonParas, string Type)
        {
            string result = "";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.Method = "POST";
            req.ContentType = Type;
            req.Accept = Type;
            req.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            req.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";
            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes(jsonParas);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp;
            try { resp = (HttpWebResponse)req.GetResponse(); }
            catch (WebException ex) { resp = (HttpWebResponse)ex.Response; }
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
