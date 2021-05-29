using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Novels
{
	public static class NovelCommon
	{
		public static string WrapNullOrString(string value)
		{
			return value == null ? NovelConsts.SERIALIZED_NULL : NovelConsts.SERIALIZED_NOT_NULL_PREFIX + value;
		}

		public static string UnwrapNullOrString(string value)
		{
			return value == NovelConsts.SERIALIZED_NULL ? null : value.Substring(NovelConsts.SERIALIZED_NOT_NULL_PREFIX.Length);
		}
	}
}
