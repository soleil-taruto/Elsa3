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
	/// 第02形態
	/// </summary>
	public class Enemy_チルノ_02 : Enemy
	{
		public Enemy_チルノ_02(double x, double y)
			: base(x, y, Kind_e.ENEMY, 6000, 0)
		{ }

		private double Target_X;
		private double Target_Y;

		protected override IEnumerable<bool> E_Draw()
		{
			// ---- 環境制御 ----

			Game.I.Walls.Add(new Wall_32021_背面());
			Game.I.Walls.Add(new Wall_32011_前面());

			// ----

			Func<bool> f_updateTarget = SCommon.Supplier(this.E_UpdateTarget());
			Func<bool> f_attack = SCommon.Supplier(this.E_Attack());

			for (int frame = 0; ; frame++)
			{
				f_updateTarget();

				const double APPR_RATE = 0.99;

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
			const int FRAME_MAX = 120;
			const double R = 200.0;

			double[] pts = new double[]
			{
				GameConsts.FIELD_W / 2 - R,
				GameConsts.FIELD_H / 2 - R,
				GameConsts.FIELD_W / 2 + R,
				GameConsts.FIELD_H / 2 - R,
				GameConsts.FIELD_W / 2 + R,
				GameConsts.FIELD_H / 2 + R,
				GameConsts.FIELD_W / 2 - R,
				GameConsts.FIELD_H / 2 + R,
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
			DDRandom rand = new DDRandom(1112);
			EnemyCommon_HenyoriLaser.LASER_COLOR_e color = EnemyCommon_HenyoriLaser.LASER_COLOR_e.RED;

			for (; ; )
			{
				rand.Next(); // 調整

				foreach (DDScene scene in DDSceneUtils.Create(10))
				{
					// 攻撃_へにょりレーザー_個別
					{
						double speed = 5.0;
						double angle = scene.Rate * Math.PI * 2.0 + Math.PI * 0.5;

						Game.I.Enemies.Add(new Enemy_チルノ_HenyoriLaser_01(
							this.X,
							this.Y,
							speed,
							angle,
							new double[]
							{
								rand.DReal() * 0.35,
								rand.DReal() * 0.25,
								rand.DReal() * 0.15,
								rand.DReal() * 0.05,
							},
							color
							));
					}

					for (int c = 0; c < 20; c++)
						yield return true;
				}

				for (int c = 0; c < 180; c++)
					yield return true;

				color = (EnemyCommon_HenyoriLaser.LASER_COLOR_e)(((int)color + 1) % EnemyCommon_HenyoriLaser.LASER_COLOR_e_NUM);

				// 攻撃_へにょりレーザー_まとめて
				{
					double[] angleRnds = new double[]
					{
						rand.DReal() * 0.35,
						rand.DReal() * 0.25,
						rand.DReal() * 0.15,
					};

					foreach (DDScene scene in DDSceneUtils.Create(10))
					{
						double speed = 7.5;
						double angle = scene.Rate * Math.PI * 2.0 + Math.PI * 0.5;

						Game.I.Enemies.Add(new Enemy_チルノ_HenyoriLaser_01(
							this.X,
							this.Y,
							speed,
							angle,
							angleRnds,
							color
							));
					}
				}

				//for (int c = 0; c < 400 && Game.I.Enemies.Iterate().Any(enemy => enemy is Enemy_HenyoriLaser); c++)
				for (int c = 0; c < 400; c++)
					yield return true;

				color = (EnemyCommon_HenyoriLaser.LASER_COLOR_e)(((int)color + 1) % EnemyCommon_HenyoriLaser.LASER_COLOR_e_NUM);
			}
		}

		protected override void Killed()
		{
			// 次の形態へ移行する。

			Ground.I.SE.SE_ENEMYKILLED.Play();

			EnemyCommon.DropItem(this, 3);
			EnemyCommon.DropItem(this, 22);

			Game.I.Enemies.Add(new Enemy_チルノ_03(this.X, this.Y));
			Game.I.Score += 85000000 * (Game.I.PlayerWasDead ? 1 : 2);
			EnemyCommon.Drawノーミス();
			Game.I.PlayerWasDead = false;
		}

		public override bool IsBoss()
		{
			return true;
		}
	}
}
