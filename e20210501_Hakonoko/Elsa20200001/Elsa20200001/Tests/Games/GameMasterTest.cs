using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameProgressMasters;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class GameMasterTest
	{
		public void Test01()
		{
			int startStageIndex;

			// ---- choose one ----

			//startStageIndex = 0; // テスト用
			//startStageIndex = 1; // 最初のステージ
			//startStageIndex = 2;
			//startStageIndex = 3;
			//startStageIndex = 4;
			//startStageIndex = 5;
			//startStageIndex = 6;
			startStageIndex = 7;
			//startStageIndex = 8;
			//startStageIndex = 9;

			// ----

			using (new GameProgressMaster())
			{
				GameProgressMaster.I.StartStageIndex = startStageIndex;
				GameProgressMaster.I.Perform();
			}
		}
	}
}
