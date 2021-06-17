using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Games.Scripts;

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

		public class StageInfo
		{
			public string Name;
			public Func<Script> CreateScript;

			public StageInfo(Func<Script> f_createScript, string name)
			{
				this.Name = name;
				this.CreateScript = f_createScript;
			}
		}

		public static StageInfo[] Stages =
		{
			new StageInfo(() => new Script_ステージ_01(), "01.通常ステージ"),
			new StageInfo(() => new Script_ステージ_02(), "02.敵弾吸収テスト_ステージ"),
			new StageInfo(() => new Script_ステージ_03(), "03.へにょりレーザー_ステージ"),

			// 後続のステージをここへ追加..
		};

		public bool RestartFlag = false;
		public bool ReturnToTitleMenu = false;

		public void Perform(int startStageIndex, Player.PlayerWho_e plWho)
		{
		restart:
			GameStatus gameStatus = new GameStatus();

			for (int stageIndex = startStageIndex; stageIndex < Stages.Length; stageIndex++)
			{
				// reset
				{
					this.RestartFlag = false;
					this.ReturnToTitleMenu = false;
				}

				using (new Game())
				{
					Game.I.Script = Stages[stageIndex].CreateScript();
					Game.I.Player.PlayerWho = plWho;
					Game.I.Status = gameStatus;
					Game.I.Perform();
				}
				if (RestartFlag)
					goto restart;

				if (ReturnToTitleMenu)
					return;
			}
		}
	}
}
