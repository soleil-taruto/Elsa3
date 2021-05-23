using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Common
	{
		public static string WrapNullOrString(string value)
		{
			return value == null ? Consts.SERIALIZED_NULL : Consts.SERIALIZED_NOT_NULL_PREFIX + value;
		}

		public static string UnwrapNullOrString(string value)
		{
			return value == Consts.SERIALIZED_NULL ? null : value.Substring(Consts.SERIALIZED_NOT_NULL_PREFIX.Length);
		}

		public static string FirstNotEmpty(params string[] strs)
		{
			return strs.First(str => !string.IsNullOrEmpty(str));
		}
	}
}
