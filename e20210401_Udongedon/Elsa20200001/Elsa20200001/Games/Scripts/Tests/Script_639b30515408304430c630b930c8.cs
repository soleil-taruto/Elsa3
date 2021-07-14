using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies;
using Charlotte.GameCommons;
using Charlotte.Games.Surfaces;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_掛け合いテスト : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_21001());
			Game.I.Walls.Add(new Wall_21002());

			string[] leftCharas = new string[]
			{
				"小悪魔",
				"メディスン",
			};

			string[] rightCharas = new string[]
			{
				"鍵山雛",
				"ルーミア",
				"チルノ",
			};

			foreach (string leftChara in leftCharas)
			{
				foreach (string rightChara in rightCharas)
				{
					string scenarioFile = string.Format(@"res\掛け合いシナリオ\{0}_{1}.txt", leftChara, rightChara);

					foreach (bool v in ScriptCommon.掛け合い(new Scenario(scenarioFile)))
						yield return v;

					// ----

					Game.I.SurfaceManager = new SurfaceManager(); // reset

					for (int c = 0; c < 120; c++) // 適当な待ち
						yield return true;
				}
			}
		}
	}
}
