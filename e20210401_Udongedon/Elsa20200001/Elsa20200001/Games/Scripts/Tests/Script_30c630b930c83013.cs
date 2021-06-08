using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;
using Charlotte.GameCommons;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_テスト3013 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			// ====

			Game.I.Enemies.Add(new Enemy_3002(-100, -100, 800, 60, 6, 0, 21, 0.0, 100.0, 1.0, 0.0, 0.98));

			for (int c = 0; c < 200; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_3002(600, -100, 800, 60, 6, 0, 22, 500, 300.0, -1.0, 0.0, 0.98));

			for (int c = 0; c < 250; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_02(-50.0, 100.0, 150, 30, 102, 22, 1.0));
			Game.I.Enemies.Add(new Enemy_JackOLantern_02(-100.0, 200.0, 150, 30, 102, 22, 1.0));

			Game.I.Enemies.Add(new Enemy_3002(-99, 600, 800, 60, 6, 0, 21, 100, 300.0, 0.0, -0.2, 0.99));
			Game.I.Enemies.Add(new Enemy_3002(600, 600, 800, 60, 6, 0, 21, 412, 300.0, 0.0, -0.2, 0.99));

			for (int c = 0; c < 200; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_02(600.0, 150.0, 150, 30, 102, 22, -1.0));
			Game.I.Enemies.Add(new Enemy_JackOLantern_02(650.0, 250.0, 150, 30, 102, 22, -1.0));

			// ====

			for (; ; )
			{
				yield return true;
			}
		}
	}
}
