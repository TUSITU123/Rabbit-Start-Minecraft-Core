using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Core.Helpers
{
    /// <summary>
    /// 更新帮助器
    /// </summary>
    public class UpdateHelper
    {
        protected string[] after;
        /// <summary>
        /// Get信息
        /// </summary>
        protected static string Get(string Url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.Proxy = null;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            httpWebResponse?.Close();
            httpWebRequest?.Abort();
            return result;
        }
        /// <summary>
        /// 获取版本编号
        /// </summary>
        /// <returns>从2018k.cn上获取的你的实例的版本编号</returns>
        public string GetVersion(string id)
        {

            new WebClient().Credentials = CredentialCache.DefaultCredentials;
            new StringBuilder();
            string text = Get("http://2018k.cn/api/checkVersion?id=" + id);
            after = text.Split('|');
            return after[4];


        }
        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <returns>从2018k.cn上获取的你的实例的更新信息</returns>
        public string GetUpdateinformation(string id)
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            StringBuilder sb = new StringBuilder();
            String pageData = MyWebClient.DownloadString("http://2018k.cn/api/getExample?id=" + id + "&data=remark"); //从指定网站下载数据
            pageData = Encoding.UTF8.GetString(MyWebClient.DownloadData("http://2018k.cn/api/getExample?id=" + id + "&data=remark"));
            return pageData;
        }
        /// <summary>
        /// 获取更新下载网址
        /// </summary>
        /// <returns>从2018k.cn上获取的你的实例的更新下载网址</returns>
        public string GetDownloadFile(string id)
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            StringBuilder sb = new StringBuilder();
            String pageData = MyWebClient.DownloadString("http://2018k.cn/api/getExample?id=" + id + "&data=url"); //从指定网站下载数据
            pageData = Encoding.UTF8.GetString(MyWebClient.DownloadData("http://2018k.cn/api/getExample?id=" + id + "&data=url"));
            return pageData;
        }
        /// <summary>
        /// 获取更新公告
        /// </summary>
        /// <returns>从2018k.cn上获取的你的实例的更新公告</returns>
        public string GetUpdateNotice(string id)
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            StringBuilder sb = new StringBuilder();
            String pageData = MyWebClient.DownloadString("http://2018k.cn/api/getExample?id=" + id + "&data=notice"); //从指定网站下载数据
            pageData = Encoding.UTF8.GetString(MyWebClient.DownloadData("http://2018k.cn/api/getExample?id=" + id + "&data=notice"));
            return pageData;
        }
    }
}
