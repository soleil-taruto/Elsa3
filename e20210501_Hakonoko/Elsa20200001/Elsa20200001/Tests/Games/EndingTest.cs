using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Novels;

namespace Charlotte.Tests.Games
{
	public class EndingTest
	{
		public void Test_死亡()
		{
			new Ending_死亡().Perform();
		}

		public void Test_生還()
		{
			new Ending_生還().Perform();
		}

		public void Test_復讐()
		{
			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("エンディング_復讐");
				Novel.I.Perform();

				if (Novel.I.会話スキップした)
					//throw new 箱から出る.Cancelled();
					return;
			}
			new Ending_復讐().Perform();
		}
	}
}
