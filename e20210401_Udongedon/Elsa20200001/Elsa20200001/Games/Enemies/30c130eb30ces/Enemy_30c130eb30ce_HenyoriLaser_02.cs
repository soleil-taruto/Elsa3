using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Enemies.チルノs
{
	public class Enemy_チルノ_HenyoriLaser_02 : Enemy_HenyoriLaser
	{
		private double AngleAdd;
		private double AngleAddMul;

		public Enemy_チルノ_HenyoriLaser_02(double x, double y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e kind, EnemyCommon_HenyoriLaser.LASER_COLOR_e color, double speed, double angle, double angleAdd, double angleAddMul, double width)
			: base(x, y, kind, color)
		{
			this.Speed = speed;
			this.Angle = angle;
			this.AngleAdd = angleAdd;
			this.AngleAddMul = angleAddMul;
			this.Width = width;
		}

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			for (; ; )
			{
				this.Angle += this.AngleAdd;
				this.AngleAdd *= this.AngleAddMul;

				yield return true;
			}
		}
	}
}
