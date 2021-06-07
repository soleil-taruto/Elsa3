﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;
using Charlotte.GameCommons;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_テスト3001 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			for (; ; )
			{
				Game.I.Enemies.Add(new Enemy_HenyoriLaser_01(200, 100, (EnemyCommon_HenyoriLaser.LASER_COLOR_e)DDUtils.Random.GetInt(7)));

				for (int w = 0; w < 60; w++)
					yield return true;

				yield return true;
			}
		}
	}
}
