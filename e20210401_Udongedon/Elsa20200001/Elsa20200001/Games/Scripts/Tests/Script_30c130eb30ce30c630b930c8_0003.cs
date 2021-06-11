﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies.チルノs;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_チルノテスト_0003 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			Game.I.Enemies.Add(new Enemy_チルノ_02(GameConsts.FIELD_W / 2, GameConsts.FIELD_H / 16));

			for (; ; )
			{
				// noop

				yield return true;
			}
		}
	}
}
