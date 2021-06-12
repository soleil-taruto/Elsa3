using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;
using Charlotte.Games.Walls;

namespace Charlotte.Games.Enemies.チルノs
{
	/// <summary>
	/// チルノ
	/// 第01形態
	/// </summary>
	public class Enemy_チルノ_01 : Enemy
	{
		public Enemy_チルノ_01(double x, double y)
			: base(x, y, Kind_e.ENEMY, 3000, 0)
		{ }

		private double Target_X;
		private double Target_Y;

		protected override IEnumerable<bool> E_Draw()
		{
			// ---- 環境制御 ----

			Game.I.Walls.Add(new Wall_32011_背面());
			Game.I.Walls.Add(new Wall_31001_前面());

			// ----

			Func<bool> f_updateTarget = SCommon.Supplier(this.E_UpdateTarget());
			Func<bool> f_attack = SCommon.Supplier(this.E_Attack());

			for (int frame = 0; ; frame++)
			{
				f_updateTarget();

				const double APPR_RATE = 0.98;

				DDUtils.Approach(ref this.X, this.Target_X, APPR_RATE);
				DDUtils.Approach(ref this.Y, this.Target_Y, APPR_RATE);

				if (EnemyConsts_チルノ.TRANS_FRAME < frame)
				{
					f_attack();
				}

				EnemyCommon_チルノ.PutCrash(this, frame);
				EnemyCommon_チルノ.Draw(this.X, this.Y);

				yield return true;
			}
		}

		private IEnumerable<bool> E_UpdateTarget()
		{
			const double MARGIN = 30.0;
			const double Y_MAX = 100.0;
			const int FRAME_MAX = 100;

			double[] pts = new double[]
			{
				MARGIN, MARGIN,
				GameConsts.FIELD_W / 2, MARGIN,
				GameConsts.FIELD_W / 2, Y_MAX,
				GameConsts.FIELD_W - MARGIN, Y_MAX,
				GameConsts.FIELD_W - MARGIN, MARGIN,
				GameConsts.FIELD_W / 2, MARGIN,
				GameConsts.FIELD_W / 2, Y_MAX,
				MARGIN, Y_MAX,
			};

			for (; ; )
			{
				for (int index = 0; index < pts.Length; index += 2)
				{
					this.Target_X = pts[index + 0];
					this.Target_Y = pts[index + 1];

					for (int c = 0; c < FRAME_MAX; c++)
						yield return true;
				}
			}
		}

		private IEnumerable<bool> E_Attack()
		{
			for (; ; )
			{
				for (int c = 0; c < 3; c++)
				{
					// 攻撃_01 ナイフ
					{
						double speed = 5.0;

						foreach (int div in new int[] { 0, 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1, 0 })
						{
							for (int cnt = 0; cnt <= div; cnt++)
							{
								double angleStep = 0.15;
								double angle = cnt * angleStep - div * angleStep / 2.0;

								Game.I.Enemies.Add(new Enemy_Tama_01(
									this.X,
									this.Y,
									EnemyCommon.TAMA_KIND_e.KNIFE,
									EnemyCommon.TAMA_COLOR_e.WHITE,
									speed,
									angle
									));
							}
							speed -= 0.2;
						}
					}

					for (int d = 0; d < 60; d++)
						yield return true;
				}

				for (int c = 0; c < 90; c++)
					yield return true;

				// 攻撃_02 へにょりレーザー
				{
					bool 画面左寄り = this.X < GameConsts.FIELD_W / 2;

					double angleRnd_01 = DDUtils.Random.DReal() * 0.01;
					double angleRnd_02 = DDUtils.Random.DReal() * 0.01;

					for (int cnt = 0; cnt < 10; cnt++)
					{
						double plAngle = DDUtils.GetAngle(new D2Point(Game.I.Player.X, Game.I.Player.Y) - new D2Point(this.X, this.Y));

						double speed = (double)cnt * 0.3 + 3.0;
						double angle = plAngle + (double)cnt * 0.13 * (画面左寄り ? -1 : 1);

						Game.I.Enemies.Add(new Enemy_チルノ_HenyoriLaser_01(
							this.X,
							this.Y,
							speed,
							angle,
							new double[]
							{
								0.02 * (画面左寄り ? -1 : 1) + angleRnd_01,
								0.06 * (画面左寄り ? 1 : -1) + angleRnd_02,
							},
							EnemyCommon_HenyoriLaser.LASER_COLOR_e.CYAN
							));
					}
				}

				for (int c = 0; c < 240; c++)
					yield return true;
			}
		}

		protected override void Killed()
		{
			// 次の形態へ移行する。

			Ground.I.SE.SE_ENEMYKILLED.Play();

			EnemyCommon.DropItem(this, 3);
			EnemyCommon.DropItem(this, 22);

			Game.I.Enemies.Add(new Enemy_チルノ_02(this.X, this.Y));
			Game.I.Score += 35000000 * (Game.I.PlayerWasDead ? 1 : 2);
			EnemyCommon.Drawノーミス();
			Game.I.PlayerWasDead = false;
		}

		public override bool IsBoss()
		{
			return true;
		}
	}
}
