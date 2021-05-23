using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class WorldGameMasterTest
	{
		public void Test01()
		{
			using (new WorldGameMaster())
			{
				WorldGameMaster.I.Perform();
			}
		}

		public void Test02()
		{
			using (new WorldGameMaster())
			{
				WorldGameMaster.I.World = new World("Stage_0001_v001\\t1001");
				WorldGameMaster.I.Status = new GameStatus();
				WorldGameMaster.I.Perform();
			}
		}

		public void Test03()
		{
			string startMapName;

			// ---- choose one ----

			//startMapName = "Stage_0001_v001\\t1001";
			//startMapName = "Stage_0001_v001\\t1002";
			//startMapName = "Stage_0001_v001\\t1003";
			//startMapName = "Stage_0001_v001\\t1004";

			startMapName = "Stage_0002_v001\\Start";
			//startMapName = "Stage_0002_v001\\Room_02";
			//startMapName = "Stage_0002_v001\\Room_03";
			//startMapName = "Stage_0002_v001\\Room_04";

			// ----

			using (new WorldGameMaster())
			{
				WorldGameMaster.I.World = new World(startMapName);
				WorldGameMaster.I.Status = new GameStatus();
				WorldGameMaster.I.Perform();
			}
		}
	}
}
