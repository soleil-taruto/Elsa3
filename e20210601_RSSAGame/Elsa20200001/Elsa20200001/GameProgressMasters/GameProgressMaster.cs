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
			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("Start");
				Novel.I.Perform();
			}
			this.StageSelectLoop(new GameStatus());
		}

		public void Perform_コンテニュー()
		{
			GameStatus gameStatus;

			using (new PasswordInput())
			{
				PasswordInput.I.Perform();

				if (PasswordInput.I.LoadedGameStatus == null) // ? パスワード入力中止
					return;

				gameStatus = PasswordInput.I.LoadedGameStatus;
			}
			this.StageSelectLoop(gameStatus);
		}

		private void StageSelectLoop(GameStatus gameStatus)
		{
			for (; ; )
			{
				int stageNo;

				using (new StageSelectMenu())
				{
					StageSelectMenu.I.GameStatus = gameStatus;
					StageSelectMenu.I.Perform();

					stageNo = StageSelectMenu.I.SelectedStageNo;
				}

				// TODO
			}
		}
	}
}
