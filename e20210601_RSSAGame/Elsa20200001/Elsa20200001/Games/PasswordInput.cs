using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class PasswordInput : IDisposable
	{
		public bool[,] Password; // null == 入力キャンセル

		// <---- ret

		public static PasswordInput I;

		public PasswordInput()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			throw null; // TODO
		}
	}
}
