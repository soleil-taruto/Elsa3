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

		public void Test03()
		{
			string name;

			// ---- choose one ----

			//name = "テスト1001";
			//name = "テスト1002";
			//name = "テスト1003";
			//name = "テスト1004";
			//name = "ステージ0001";
			//name = "ステージ0002";
			//name = "ステージ0003";
			//name = "ステージ0004";
			//name = "ステージ0005";
			//name = "ステージ0006";
			//name = "ステージ0007";
			name = "ステージ0008";
			//name = "ステージ0009";
			//name = "エンディング_復讐";

			// ----

			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario(name);
				Novel.I.Perform();
			}
		}
	}
}
