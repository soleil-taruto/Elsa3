using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies.チルノs;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_チルノテスト_0001 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			{
				Enemy_チルノ boss;

				Game.I.Enemies.Add(boss = new Enemy_チルノ());

				for (int c = 0; c < 30; c++)
					yield return true;

				foreach (bool v in ScriptCommon.掛け合い(new Scenario(@"res\掛け合いシナリオ\メディスン_チルノ.txt")))
					yield return v;

				boss.NextFlag = true;
			}

			Ground.I.Music.MUS_BOSS_03.Play();

			for (; ; )
			{
				// noop

				yield return true;
			}
		}
	}
}
