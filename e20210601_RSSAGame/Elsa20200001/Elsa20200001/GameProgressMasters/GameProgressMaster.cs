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
			using (new StageSelectMenu())
			{
				StageSelectMenu.I.GameStatus = new GameStatus();
				StageSelectMenu.I.Perform();
			}
		}

		public void Perform_コンテニュー()
		{
			bool[,] password;

			using (new PasswordInput())
			{
				PasswordInput.I.Perform();

				if (PasswordInput.I.Password == null)
					return;

				password = PasswordInput.I.Password;
			}
			GameStatus gameStatus = GameStatus.Deserialize(GameStatus.PasswordConv.GetValue(password));

			using (new StageSelectMenu())
			{
				StageSelectMenu.I.GameStatus = gameStatus;
				StageSelectMenu.I.Perform();
			}
		}
	}
}
