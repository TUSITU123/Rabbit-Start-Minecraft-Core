using FastX.Class.Interfaces;
using FastX.Core.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Models
{
    public class LaunchErrorModel
    {
        /// <summary>
        /// 错误的详细信息
        /// </summary>
        public string ErrorMessages { get; set; }

        /// <summary>
        /// 导致错误的可能原因
        /// </summary>
        public string Causes { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public enum ErrorType 
        {
            /// <summary>
            /// 某个参数错误
            /// </summary>
            None,
            /// <summary>
            /// 没有Java或Java路径不存在
            /// </summary>
            NoJava,
            /// <summary>
            /// 解压缩natives失败
            /// </summary>
            DecompressFailed,
            /// <summary>
            /// 验证失败
            /// </summary>
            AuthFailed,
            /// <summary>
            /// 操作失败
            /// </summary>
            OperationFailed,
            /// <summary>
            /// 不完全的jvm参数
            /// </summary>
            IncompleteArguments,
            /// <summary>
            /// 未知错误
            /// </summary>
            Unknown
        }
    }
    /// <summary>
    /// 启动设置
    /// </summary>
    public class LaunchModel
    {
        /// <summary>
        /// 验证方式
        /// </summary>
        public LaunchTypeModel Authenticator { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 游戏窗口宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 游戏窗口高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 添加jvm参数
        /// </summary>
        public string AddJVMparameters { get; set; }
        /// <summary>
        /// Java路径
        /// </summary>
        public string JavaExecutable { get; set; }
        /// <summary>
        /// 启动游戏所需的最大内存
        /// </summary>
        public int Maxmemory { get; set; }
        /// <summary>
        /// 启动游戏所需的最小内存 (可选，默认512)
        /// </summary>
        public int Minimemory { get; set; }
        /// <summary>
        /// 游戏窗口标题
        /// </summary>
        public string WindowTitle { get; set; }
        /// <summary>
        /// 外置验证参数
        /// </summary>
        public string ParameterofYggdrasil { get; set; }
        /// <summary>
        /// 启动水印
        /// </summary>
        public string LauncherName { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 玩家的Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 玩家的uuid
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// 服务器IP，可用于启动直连
        /// </summary>
        public string ServerIp { get; set; }
        /// <summary>
        /// 服务器端口，与ServerIP配套(服务器默认为25565)
        /// </summary>
        public int ServerPort { get; set; }

        //    public AuthenticatorofInterface Authenticator { get; set; }

    }
}
