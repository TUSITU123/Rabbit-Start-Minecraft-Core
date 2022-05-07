using FastX.Class.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastX.Core.Authenticator
{
    public class OfflineAuthenticator : AuthenticatorofInterface
    {
        /// <summary>
        ///     获取或设置用户名。
        /// </summary>
        public string Username { get; set; }
        public readonly string Password;
        public OfflineAuthenticator()
        {
            Password = Username;
        }

        public string Type
        {
            get { return "Offline"; }
        }
    }
}

