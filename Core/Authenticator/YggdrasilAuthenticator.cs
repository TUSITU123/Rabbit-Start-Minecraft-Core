using BlessingSkinJson;
using FastX.Class.Models;
using Newtonsoft.Json;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static FastX.Class.Models.AuthenticatorModel;


namespace FastX.Core.Authenticator
{

        /// <summary>
        /// 第三方外置服务器验证器
        /// </summary>
        public class YggdrasilAuthenticator
        {
            public YggdrasilAuthenticator(string str1)
            {

            }
            #region 内部方法
            private ErrorModels error = new ErrorModels();
            private Download web = new Download();
            #endregion
            /// <summary>
            /// 外置账号验证方法
            /// </summary>
            /// <param name="url">皮肤站的认证地址</param>
            /// <param name="UserEmail">账号邮箱</param>
            /// <param name="Password">账号的密码</param>
            /// <returns></returns>
            public YggdrasilAuthenticatorModel AccountofYggdrasil(string uri, string UserEmail, string Password)
            {
                string str = this.web.Post(uri + "/authserver/authenticate", "{\"username\":\"" + UserEmail + "\",\"password\":\"" + Password + "\"}");
                BlessingSkin.Root root = new BlessingSkin.Root();
                try
                {
                    root = JsonConvert.DeserializeObject<BlessingSkin.Root>(str);
                }
                catch (Exception)
                {
                    error.ErrorMessages = "外置服务器网址错误或不存在！";
                    error.Causes = "手残打错了或域名已过期";
                }
                if (root == null)
                {
                    error.ErrorMessages = "网络异常！";
                    error.Causes = "请检查网络是否正常连接";
                }
                if (root.accessToken == null)
                {
                    error.ErrorMessages = "未知异常！";
                    error.Causes = Regex.Unescape(JsonConvert.DeserializeObject<BlessingSkinError>(str).errorMessage);
                }
                YggdrasilAuthenticatorModel skin = new YggdrasilAuthenticatorModel
                {
                    accessToken = root.accessToken
                };
                List<YggdrasilAttributetype> list = new List<YggdrasilAttributetype>();
                foreach (BlessingSkin.AvailableProfilesItem item in root.availableProfiles)
                {
                    YggdrasilAttributetype name = new YggdrasilAttributetype
                    {
                        Name = item.name,
                        uuid = item.id
                    };
                    list.Add(name);
                }
                skin.NameItem = list.ToArray();
                return skin;
            }
            public string Name { get; set; }
            public string Uuid { get; set; }
            public string Token { get; set; }
        }
}
