using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Models
{
    public class ServerPingModel
    {
        /// <summary>
        /// 服务器版本
        /// </summary>
        public Version version { get; set; }

        /// <summary>
        /// 服务器玩家
        /// </summary>
        public Players Players { get; set; }

        /// <summary>
        /// 服务器信息
        /// </summary>
        public Description Description { get; set; }

        /// <summary>
        /// 服务器Mod信息
        /// </summary>
        public Modinfo Modinfo { get; set; }

        /// <summary>
        /// 服务器图标
        /// </summary>
        public string Favicon { get; set; } = null;

        /// <summary>
        /// 错误信息（如果有）
        /// </summary>
        public string Error { get; set; } = null;

    }
    #region 内部类集合
    public class Version
    {
        public string name { get; set; }
        public int protocol { get; set; }
    }

    public class Players
    {
        public int max { get; set; }
        public int online { get; set; }
        public List<object> sample { get; set; }
    }

    public class Extra
    {
        public string color { get; set; }
        public string text { get; set; }
        public bool? strikethrough { get; set; }
        public bool? bold { get; set; }
    }

    public class Description
    {
        public List<Extra> extra { get; set; }
        public string text { get; set; }
    }

    public class Modinfo
    {
        public string type { get; set; }
        public List<object> modList { get; set; }
    }

    #endregion
}
