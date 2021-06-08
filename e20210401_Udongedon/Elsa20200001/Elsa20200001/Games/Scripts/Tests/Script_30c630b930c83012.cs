using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;
using Charlotte.GameCommons;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_テスト3012 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			Game.I.Enemies.Add(new Enemy_3001(-100, -100, 1000, 60, 4, 0, 21, 0.0, 100.0, 1.0, 0.0, 0.98));

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_3001(GameConsts.FIELD_W + 100, -100, 1000, 60, 4, 0, 22, GameConsts.FIELD_W, 300.0, -1.0, 0.0, 0.98));

			for (; ; )
			{
				yield return true;
			}
		}
	}
}
