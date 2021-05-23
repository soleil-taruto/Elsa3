using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Novels;

namespace Charlotte.Tests.Novels
{
	public class NovelTest
	{
		public void Test01()
		{
			using (new Novel())
			{
				Novel.I.Perform();
			}
		}

		public void Test02()
		{
			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("テスト0001");
				Novel.I.Perform();
			}
		}
	}
}
