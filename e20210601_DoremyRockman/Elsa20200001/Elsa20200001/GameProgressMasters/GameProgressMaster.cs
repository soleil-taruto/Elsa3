using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Novels;

namespace Charlotte.GameProgressMasters
{
	public class GameProgressMaster : IDisposable
	{
		public static GameProgressMaster I;

		public GameProgressMaster()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			// zantei zantei zantei
			// zantei zantei zantei
			// zantei zantei zantei

			// TODO
			// TODO
			// TODO

			//using (new Novel())
			//{
			//    Novel.I.Status.Scenario = new Scenario("101_ゲームスタート");
			//    Novel.I.Perform();
			//}
			//using (new WorldGameMaster())
			//{
			//    WorldGameMaster.I.World = new World("w0001\\t0001"); // 仮？
			//    WorldGameMaster.I.Status = new GameStatus();
			//    WorldGameMaster.I.Perform();
			//}
		}

		public void Perform_コンテニュー()
		{
			// zantei zantei zantei
			// zantei zantei zantei
			// zantei zantei zantei

			// TODO
			// TODO
			// TODO
		}
	}
}
