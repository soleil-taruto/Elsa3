using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Common
	{
		public static T GetElement<T>(T[] arr, int index, T defval)
		{
			if (index < arr.Length)
			{
				return arr[index];
			}
			else
			{
				return defval;
			}
		}
	}
}
