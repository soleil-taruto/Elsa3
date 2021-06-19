using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_テスト3031 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			// ====

			for (int c = 0; c < 300; c++)
				yield return true;

			// All Clear Bonus
			{
				long bonus = 100000000;

				DDGround.EL.Add(SCommon.Supplier(Effects.Message(
					"ALL CLEAR BONUS +" + bonus,
					new I3Color(64, 64, 0),
					new I3Color(255, 255, 0)
					)));

				Game.I.Score += bonus;
			}

			if (!Game.I.Status.PlayerWasBombUsed) // ボム無使用ボーナス
			{
				for (int c = 0; c < 240; c++)
					yield return true;

				long bonus = 100000000;

				DDGround.EL.Add(SCommon.Supplier(Effects.Message(
					"ボ ム 無 使 用 ボ ー ナ ス +" + bonus,
					new I3Color(128, 0, 64),
					new I3Color(0, 255, 255)
					)));

				Game.I.Score += bonus;
			}

			for (int c = 0; c < 300; c++)
				yield return true;

			// ====
		}
	}
}
