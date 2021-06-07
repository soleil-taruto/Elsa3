using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;
using Charlotte.GameCommons;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_テスト3011 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			Game.I.Enemies.Add(new Enemy_JackOLantern_03(GameConsts.FIELD_W / 2, -100.0, 30, 10, 100, 0, 50.0, 2.0, 0.0, 0.1));

			for (; ; )
			{
				for (int c = 0; c < 90; c++)
					yield return true;

				yield return true;

				// ----

				Game.I.Enemies.Add(new Enemy_JackOLantern_03(DDUtils.Random.Real() * GameConsts.FIELD_W, -100.0, 30, 10, 100, 0, 50.0, 2.0, 0.0, 0.1));
			}
		}
	}
}
