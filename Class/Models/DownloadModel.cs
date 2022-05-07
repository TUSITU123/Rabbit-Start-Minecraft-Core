using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Models
{
    public class DownloadModel
    {
        /// <summary>
        /// 下载网址
        /// </summary>
        public string Url { get; internal set; }
        /// <summary>
        /// 下载路径
        /// </summary>
        public string path { get; internal set; }
        internal string name { get; set; }
        internal string mainClass { get; set; }

    }
    public class VersionsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string releaseTime { get; set; }
    }
    public class Root
    {
        public List<VersionsItem> versions { get; set; }
    }
    public class VersionList
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string id { get; internal set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; internal set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string releaseTime { get; internal set; }
    }

}
