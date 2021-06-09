using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// へにょりレーザー妖精
	/// 放射レーザー
	/// </summary>
	public class Enemy_3001 : Enemy
	{
		private EnemyCommon.FairyInfo Fairy;
		//private int ShotType;
		private int DropItemMode;
		private double TargetX;
		private double TargetY;
		private double XAdd;
		private double YAdd;
		private double ApproachingRate;

		public Enemy_3001(double x, double y, int hp, int transFrame, int fairyKind, int shotType, int dropItemType, double targetX, double targetY, double xAdd, double yAdd, double approachingRate)
			: base(x, y, Kind_e.ENEMY, hp, transFrame)
		{
			if (shotType != 0) throw null; // 不使用なので常にゼロを指定すること。

			this.Fairy = new EnemyCommon.FairyInfo()
			{
				Enemy = this,
				Kind = fairyKind,
			};
			//this.ShotType = shotType;
			this.DropItemMode = dropItemType;
			this.TargetX = targetX;
			this.TargetY = targetY;
			this.XAdd = xAdd;
			this.YAdd = yAdd;
			this.ApproachingRate = approachingRate;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			Func<bool> a_attack = DDUtils.Scripter(this.E_Attack());

			for (int frame = 0; ; frame++)
			{
				a_attack();

				this.TargetX += this.XAdd;
				this.TargetY += this.YAdd;

				DDUtils.Approach(ref this.X, this.TargetX, this.ApproachingRate);
				DDUtils.Approach(ref this.Y, this.TargetY, this.ApproachingRate);

				//EnemyCommon.Shoot(this, this.ShotType); // 不使用

				yield return EnemyCommon.FairyDraw(this.Fairy);
			}
		}

		private IEnumerable<int> E_Attack()
		{
			yield return 120; // 移動(初回)

			for (; ; )
			{
				double x = this.X;
				double y = this.Y;

				const int SHOOT_NUM = 24;

				double bkApproachingRate = this.ApproachingRate;
				this.ApproachingRate = 0.9999;

				// 射撃
				for (int c = 0; c < SHOOT_NUM; c++)
				{
					EnemyCommon_HenyoriLaser.LASER_COLOR_e color =
						c % 2 == 0 ?
						EnemyCommon_HenyoriLaser.LASER_COLOR_e.YELLOW :
						EnemyCommon_HenyoriLaser.LASER_COLOR_e.PURPLE;

					Game.I.Enemies.Add(new Enemy_HenyoriLaser_03(x, y, color, Math.PI * 0.5 + ((double)c / SHOOT_NUM) * Math.PI * 2.0));

					yield return 5; // 射撃_間隔
				}
				this.ApproachingRate = bkApproachingRate; // restore

				yield return 60; // 移動
			}
		}

		protected override void Killed()
		{
			EnemyCommon.Killed(this, this.DropItemMode);
			Game.I.Score += 9000;
		}
	}
}
