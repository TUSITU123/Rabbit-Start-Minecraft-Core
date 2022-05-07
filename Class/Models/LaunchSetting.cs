using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Models
{
    /// <summary>
    /// 服务器启动参数配置类
    /// </summary>
    public class LaunchSetting
    {
        /// <summary>
        /// 服务器核心路径
        /// </summary>
        public string ServerPath{ get; set; }
        /// <summary>
        /// 服务器最大内存
        /// </summary>
        public int Maxmemory { get; set; }
        /// <summary>
        /// 服务器最小内存
        /// </summary>
        public int Minimemory { get; set; }
        /// <summary>
        /// 是否自动签订Eula协议
        /// </summary>
        public bool WriteEula { get; set; }

    }
}
