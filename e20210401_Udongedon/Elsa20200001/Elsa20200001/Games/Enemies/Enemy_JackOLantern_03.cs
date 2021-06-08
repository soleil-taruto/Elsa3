using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_JackOLantern_03 : Enemy
	{
		//private int ShotType;
		private int DropItemMode;
		private double XRate;
		private double YAdd;
		private double Rot;
		private double RotAdd;

		public Enemy_JackOLantern_03(double x, double y, int hp, int transFrame, int shotType, int dropItemType, double xRate, double yAdd, double rot, double rotAdd)
			: base(x, y, Kind_e.ENEMY, hp, transFrame)
		{
			if (shotType != 0) throw null; // 不使用なので常にゼロを指定すること。

			//this.ShotType = shotType;
			this.DropItemMode = dropItemType;
			this.XRate = xRate;
			this.YAdd = yAdd;
			this.Rot = rot;
			this.RotAdd = rotAdd;
		}

		private bool FreezeFlag = false;

		protected override IEnumerable<bool> E_Draw()
		{
			Func<bool> a_attack = DDUtils.Scripter(this.E_Attack());

			double axisX = this.X;

			for (int frame = 0; ; frame++)
			{
				a_attack();

				if (!this.FreezeFlag)
				{
					this.X = axisX + Math.Sin(this.Rot) * this.XRate;
					this.Y += this.YAdd;
					this.Rot += this.RotAdd;
				}

				//EnemyCommon.Shoot(this, this.ShotType); // 不使用 -> E_Attack

				int koma = frame / 7;
				koma %= 2;

				DDDraw.SetMosaic();
				DDDraw.DrawBegin(Ground.I.Picture2.D_PUMPKIN_00_ARBG[koma], this.X, this.Y);
				DDDraw.DrawZoom(2.0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X - 1.0, this.Y + 3.0), 30.0);

				yield return !EnemyCommon.IsEvacuated(this);
			}
		}

		private IEnumerable<int> E_Attack()
		{
			yield return 120; // 移動(初回)

			for (; ; )
			{
				this.FreezeFlag = true;
				yield return 10; // 射撃_前_待ち

				// 射撃
				for (int c = 0; c < 4; c++)
					Game.I.Enemies.Add(new Enemy_HenyoriLaser_02(this.X, this.Y, EnemyCommon_HenyoriLaser.LASER_COLOR_e.GREEN, c * Math.PI * 0.5));

				yield return 20; // 射撃_後_待ち
				this.FreezeFlag = false;
				yield return 60; // 移動
			}
		}

		protected override void Killed()
		{
			EnemyCommon.Killed(this, this.DropItemMode);
			Game.I.Score += 14000;
		}
	}
}
