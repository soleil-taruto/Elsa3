using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Enemies.チルノs;
using Charlotte.Games.Shots;
using Charlotte.Games.Walls;

namespace Charlotte.Games.Scripts
{
	public class Script_ステージ_03 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Ground.I.Music.MUS_STAGE_03.Play();

			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_31001_背面());
			Game.I.Walls.Add(new Wall_31001_前面());

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_0001(200.0, -50.0, 1, 30, 5, 100, 22, 4.0, -1, 350.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(300.0, -100.0, 1, 30, 5, 100, 22, 4.0, -1, 350.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(400.0, -150.0, 1, 30, 5, 100, 22, 4.0, -1, 350.0, 0.97));

			for (int c = 0; c < 240; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_03(100, -100.0, 200, 10, 0, 11, 50.0, 2.0, 0.0, 0.1));
			Game.I.Enemies.Add(new Enemy_JackOLantern_03(412, -100.0, 200, 10, 0, 12, 50.0, 2.0, 0.0, 0.1));

			for (int c = 0; c < 90; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_03(200, -100.0, 200, 10, 0, 12, 50.0, 2.0, 0.0, 0.1));
			Game.I.Enemies.Add(new Enemy_JackOLantern_03(312, -100.0, 200, 10, 0, 11, 50.0, 2.0, 0.0, 0.1));

			for (int c = 0; c < 90; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_03(100, -100.0, 200, 10, 0, 21, 50.0, 2.0, 0.0, 0.1));
			Game.I.Enemies.Add(new Enemy_JackOLantern_03(412, -100.0, 200, 10, 0, 22, 50.0, 2.0, 0.0, 0.1));

			for (int c = 0; c < 240; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_0001(472.0, -50.0, 1, 10, 0, 100, 2, 4.0, -1, 350.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(472.0, -100.0, 1, 10, 0, 100, 2, 4.0, -1, 350.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(472.0, -150.0, 1, 10, 0, 100, 2, 4.0, -1, 350.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(472.0, -200.0, 1, 10, 0, 100, 2, 4.0, -1, 350.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(472.0, -250.0, 1, 10, 0, 100, 2, 4.0, -1, 350.0, 0.97));

			for (int c = 0; c < 120; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_0001(40.0, -50.0, 1, 30, 1, 101, 2, 4.0, 1, 250.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(40.0, -100.0, 1, 30, 1, 101, 2, 4.0, 1, 250.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(40.0, -150.0, 1, 30, 1, 101, 2, 4.0, 1, 250.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(40.0, -200.0, 1, 30, 1, 101, 2, 4.0, 1, 250.0, 0.97));
			Game.I.Enemies.Add(new Enemy_0001(40.0, -250.0, 1, 30, 1, 101, 2, 4.0, 1, 250.0, 0.97));

			for (int c = 0; c < 180; c++)
				yield return true;

			// ---- Script_テスト3012

			Game.I.Enemies.Add(new Enemy_3001(-100, -100, 300, 60, 4, 0, 21, 0.0, 100.0, 1.0, 0.0, 0.98));

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_3001(GameConsts.FIELD_W + 100, -100, 300, 60, 4, 0, 22, GameConsts.FIELD_W, 300.0, -1.0, 0.0, 0.98));

			// ----

			for (int c = 0; c < 300; c++)
				yield return true;

			// ---- Script_テスト3013

			Game.I.Enemies.Add(new Enemy_3002(-100, -100, 800, 60, 6, 0, 21, 0.0, 100.0, 1.0, 0.0, 0.98));

			for (int c = 0; c < 200; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_3002(600, -100, 800, 60, 6, 0, 22, 500, 300.0, -1.0, 0.0, 0.98));

			for (int c = 0; c < 250; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_02(-50.0, 100.0, 150, 30, 102, 22, 1.0));
			Game.I.Enemies.Add(new Enemy_JackOLantern_02(-100.0, 200.0, 150, 30, 102, 3, 1.0));

			Game.I.Enemies.Add(new Enemy_3002(-99, 600, 800, 60, 6, 0, 21, 100, 300.0, 0.0, -0.2, 0.99));
			Game.I.Enemies.Add(new Enemy_3002(600, 600, 800, 60, 6, 0, 21, 412, 300.0, 0.0, -0.2, 0.99));

			for (int c = 0; c < 200; c++)
				yield return true;

			Game.I.Enemies.Add(new Enemy_JackOLantern_02(600.0, 150.0, 150, 30, 102, 3, -1.0));
			Game.I.Enemies.Add(new Enemy_JackOLantern_02(650.0, 250.0, 150, 30, 102, 22, -1.0));

			// ----

			for (int c = 0; c < 720; c++)
				yield return true;

			// ---- BOSS 登場

			Game.I.Walls.Add(new Wall_32001_背面());
			Game.I.Walls.Add(new Wall_32001_前面());

			{
				Game.I.Shots.Add(new Shot_BossBomb());

				Enemy_チルノ boss = new Enemy_チルノ();

				Game.I.Enemies.Add(boss);

				for (int c = 0; c < 90; c++)
					yield return true;

				string scenarioFile;

				switch (Game.I.Player.PlayerWho)
				{
					case Player.PlayerWho_e.メディスン:
						scenarioFile = @"res\掛け合いシナリオ\メディスン_チルノ.txt";
						break;

					case Player.PlayerWho_e.小悪魔:
						scenarioFile = @"res\掛け合いシナリオ\小悪魔_チルノ.txt";
						break;

					default:
						throw null; // never
				}
				foreach (bool v in ScriptCommon.掛け合い(new Scenario(scenarioFile)))
					yield return v;

				boss.NextFlag = true;

				// boss はすぐに消滅することに注意
			}

			Ground.I.Music.MUS_BOSS_03.Play();

			while (!Game.I.BossKilled)
				yield return true;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.Shots.Add(new Shot_BossBomb());

			// ---- BOSS 撃破

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

			for (int c = 0; c < 300; c++)
				yield return true;
		}
	}
}
