using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Models
{
    /// <summary>
    /// 通用的错误类型显示
    /// </summary>
    public class ErrorModels
    {
        /// <summary>
        /// 错误的详细信息
        /// </summary>
        public string ErrorMessages { get; set; }

        /// <summary>
        /// 导致错误的可能原因
        /// </summary>
        public string Causes { get; set; }
    }
}
