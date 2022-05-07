using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Models
{
    /// <summary>
    /// 游戏账号验证器的属性
    /// </summary>
    public class AuthenticatorModel
    {
        /// <summary>
        /// 第三方账号验证器的属性
        /// </summary>
        public class YggdrasilAuthenticatorModel//skin
        {
            /// <summary>
            /// Token
            /// </summary>
            public string accessToken { get; internal set; }
            /// <summary>
            /// 用户在皮肤站注册的所有游戏账号
            /// </summary>
            public YggdrasilAttributetype[] NameItem { get; internal set; }

        }
        /// <summary>
        /// 第三方账号属性类型
        /// </summary>
        public class YggdrasilAttributetype//skinname
        {
            /// <summary>
            /// 游戏名
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// 用户的uuid
            /// </summary>
            public string uuid { get; internal set; }
        }
        /// <summary>
        /// 微软账号验证器的属性
        /// </summary>
        public class MicrosoftAuthenticatorModel
        {
            /// <summary>
            /// 游戏名
            /// </summary>
            public string Name { get;  set; }
            /// <summary>
            /// 用户的uuid
            /// </summary>
            public string uuid { get;  set; }
            /// <summary>
            /// 用户的token
            /// </summary>
            public string token { get; set; }

        }
    }
}
