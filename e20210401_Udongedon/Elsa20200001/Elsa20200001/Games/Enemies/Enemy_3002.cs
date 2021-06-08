using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// へにょりレーザー妖精_v2
	/// 自機狙いレーザー
	/// </summary>
	public class Enemy_3002 : Enemy
	{
		private EnemyCommon.FairyInfo Fairy;
		//private int ShotType;
		private int DropItemMode;
		private double TargetX;
		private double TargetY;
		private double XAdd;
		private double YAdd;
		private double ApproachingRate;

		public Enemy_3002(double x, double y, int hp, int transFrame, int fairyKind, int shotType, int dropItemType, double targetX, double targetY, double xAdd, double yAdd, double approachingRate)
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

				Game.I.Enemies.Add(new Enemy_HenyoriLaser_04(x, y, EnemyCommon_HenyoriLaser.LASER_COLOR_e.BLUE));

				yield return 10; // 移動
			}
		}

		protected override void Killed()
		{
			EnemyCommon.Killed(this, this.DropItemMode);
			Game.I.Score += 9000;
		}
	}
}
