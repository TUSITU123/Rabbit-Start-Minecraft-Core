using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Class.Interfaces
{
	/// <summary>
	///     验证器接口
	/// </summary>
	public interface AuthenticatorofInterface
	{
		/// <summary>
		///     获取验证器的类型
		/// </summary>
		string Type { get; }
	}
}
