using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_テスト3021 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			// ====

			Game.I.Enemies.Add(new Enemy_0001(150.0, -50.0, 1, 60, 4, 112, 1, 1.5, -1, 400.0, 0.99));
			Game.I.Enemies.Add(new Enemy_0001(350.0, -100.0, 1, 60, 4, 112, 1, 1.5, 1, 400.0, 0.99));

			// ====

			for (; ; )
			{
				yield return true;
			}
		}
	}
}
