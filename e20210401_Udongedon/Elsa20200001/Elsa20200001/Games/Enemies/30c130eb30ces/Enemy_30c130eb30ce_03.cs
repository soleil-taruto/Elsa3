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
	public class Enemy_チルノ_03 : Enemy
	{
		public Enemy_チルノ_03(double x, double y)
			: base(x, y, Kind_e.ENEMY, 6000, 0)
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

				// タレット配置
				{
					const double MARGIN = 50.0;

					if (frame == 90)
					{
						Game.I.Enemies.Add(new Enemy_Turret(
							this.X,
							this.Y,
							MARGIN,
							MARGIN,
							20.0,
							Ground.I.Picture2.D_DVDM_BULLET_03[1],
							123
							));

						Game.I.Enemies.Add(new Enemy_Turret(
							this.X,
							this.Y,
							GameConsts.FIELD_W - MARGIN,
							MARGIN,
							20.0,
							Ground.I.Picture2.D_DVDM_BULLET_03[4],
							124
							));
					}
				}

				const double APPR_RATE = 0.997;

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
			const int FRAME_MAX = 60;

			double[] pts = new double[]
			{
				0, GameConsts.FIELD_H / 2, // 左
				GameConsts.FIELD_W / 2, 0, // 上
				GameConsts.FIELD_W, GameConsts.FIELD_H / 2, // 右
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
			DDRandom rand = new DDRandom(1113);

			for (; ; )
			{
				for (int c = 0; c < 3; c++)
				{
					foreach (DDScene scene in DDSceneUtils.Create(16))
					{
						double angle = scene.Rate * Math.PI * 2.0 + Math.PI * 0.5;

						for (int dir = 0; dir < 2; dir++)
						{
							Game.I.Enemies.Add(new Enemy_チルノ_HenyoriLaser_02(
								this.X,
								this.Y,
								EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.SHORT,
								new[] { EnemyCommon_HenyoriLaser.LASER_COLOR_e.GREEN, EnemyCommon_HenyoriLaser.LASER_COLOR_e.YELLOW }[dir],
								5.0,
								angle,
								0.1 * new[] { -1, 1 }[dir],
								0.99,
								17.0
								));
						}
					}

					for (int wait = 0; wait < 60; wait++)
						yield return true;
				}

				for (int wait = 0; wait < 180; wait++)
					yield return true;

				for (int c = 0; c < 5; c++)
				{
					foreach (DDScene scene in DDSceneUtils.Create(new[] { 7, 11, 13, 17, 19 }[c]))
					{
						double angle = scene.Rate * Math.PI * 2.0 + Math.PI * 0.5;

						Game.I.Enemies.Add(new Enemy_チルノ_HenyoriLaser_02(
							this.X,
							this.Y,
							EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.MIDDLE,
							EnemyCommon_HenyoriLaser.LASER_COLOR_e.PURPLE,
							7.0,
							angle,
							0.2,
							0.98,
							17.0
							));
					}

					for (int wait = 0; wait < 60; wait++)
						yield return true;
				}

				for (int wait = 0; wait < 180; wait++)
					yield return true;

				for (int c = 0; c < 4; c++)
				{
					foreach (DDScene scene in DDSceneUtils.Create(c % 2 == 0 ? 10 : 12))
					{
						double angle = scene.Rate * Math.PI * 2.0 + Math.PI * 0.5;

						Game.I.Enemies.Add(new Enemy_チルノ_HenyoriLaser_02(
							this.X,
							this.Y,
							EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.LONG,
							EnemyCommon_HenyoriLaser.LASER_COLOR_e.WHITE,
							9.0,
							angle,
							-0.3,
							0.95,
							17.0
							));
					}

					for (int wait = 0; wait < 60; wait++)
						yield return true;
				}

				for (int wait = 0; wait < 180; wait++)
					yield return true;
			}
		}

		protected override void Killed()
		{
			// 次の形態へ移行する。

			Ground.I.SE.SE_ENEMYKILLED.Play();

			EnemyCommon.Killed(this, 21);

			Game.I.BossKilled = true;
			Game.I.Shots.Add(new Shot_BossBomb());
			Game.I.Score += 8500000 * (Game.I.PlayerWasDead ? 1 : 2);
			EnemyCommon.Drawノーミス();
			Game.I.PlayerWasDead = false;
		}

		public override bool IsBoss()
		{
			return true;
		}
	}
}
