using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// タレット
	/// </summary>
	public class Enemy_Turret : Enemy
	{
		private double Target_X;
		private double Target_Y;
		private double R;
		private DDPicture Picture;
		private int ShotType;

		public Enemy_Turret(double x, double y, double targetX, double targetY, double r, DDPicture picture, int shotType)
			: base(x, y, Kind_e.TAMA, 0, 0)
		{
			this.Target_X = targetX;
			this.Target_Y = targetY;
			this.R = r;
			this.Picture = picture;
			this.ShotType = shotType;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				DDUtils.Approach(ref this.X, this.Target_X, 0.977);
				DDUtils.Approach(ref this.Y, this.Target_Y, 0.973);

				EnemyCommon.Shoot(this, this.ShotType);

				DDDraw.DrawBegin(this.Picture, this.X, this.Y);
				DDDraw.DrawRotate(DDEngine.ProcFrame * 0.01);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), this.R);

				yield return true;
			}
		}

		protected override void Killed()
		{
			EnemyCommon.Killed(this, 0);
		}
	}
}
