using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.チルノs
{
	public static class EnemyCommon_チルノ
	{
		public static void PutCrash(Enemy enemy, int frame)
		{
			if (frame == 0)
			{
				Game.I.Shots.Add(new Shot_BossBomb());
			}
			else if (frame < EnemyConsts_チルノ.BOSS_BOMB_FRAME)
			{
				// noop
			}
			else if (frame < EnemyConsts_チルノ.TRANS_FRAME)
			{
				Game.I.Shots.RemoveAll(v => v.Kind == Shot.Kind_e.BOMB); // ボム消し
				//Game.I.BombUsed = false; // 念のためリセット
				Game.I.PlayerWasDead = false; // 念のためリセット
			}
			else
			{
				enemy.Crash = DDCrashUtils.Circle(new D2Point(enemy.X, enemy.Y), 25.0);
			}
			DrawOther(enemy);
		}

		private static double Last_X = GameConsts.FIELD_W / 2;

		public static void Draw(double x, double y)
		{
			int xMoveFrame = 0;
			int picIndex;

			{
				const double MARGIN = 2.0;

				if (x < Last_X - MARGIN)
				{
					xMoveFrame++;
					picIndex = 8 + Math.Min(xMoveFrame / 10, 1);
				}
				else if (Last_X + MARGIN < x)
				{
					xMoveFrame++;
					picIndex = 10 + Math.Min(xMoveFrame / 10, 1);
				}
				else
				{
					xMoveFrame = 0;
					picIndex = DDEngine.ProcFrame / 10 % 6;
				}
			}

			Last_X = x;

			DDDraw.DrawCenter(Ground.I.Picture2.チルノ[picIndex], x, y);
		}

		public static void DrawOther(Enemy enemy)
		{
			DDGround.EL.Add(() =>
			{
				Draw水平体力ゲージ((double)enemy.HP / enemy.InitialHP);
				return false;
			});

			EnemyCommon.DrawBossPosition(enemy.X);
		}

		private static void Draw水平体力ゲージ(double hp)
		{
			const int L = 20;
			const int T = 20;
			const int W = DDConsts.Screen_W - 40;
			const int H = 20;

			int rem_w = (int)(W * hp);
			int emp_w = W - rem_w;

			const double a = 0.5;

			if (1 <= rem_w)
			{
				DDDraw.SetAlpha(a);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, L, T, rem_w, H);
				DDDraw.Reset();
			}
			if (1 <= emp_w)
			{
				DDDraw.SetAlpha(a);
				DDDraw.SetBright(1.0, 0.0, 0.0);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, L + rem_w, T, emp_w, H);
				DDDraw.Reset();
			}
		}
	}
}
