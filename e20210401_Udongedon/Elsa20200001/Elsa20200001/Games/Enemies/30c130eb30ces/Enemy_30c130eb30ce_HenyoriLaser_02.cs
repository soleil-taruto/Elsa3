using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Enemies.チルノs
{
	public class Enemy_チルノ_HenyoriLaser_02 : Enemy_HenyoriLaser
	{
		public Enemy_チルノ_HenyoriLaser_02(double x, double y, double speed, double angle)
			: base(x, y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.LONG, EnemyCommon_HenyoriLaser.LASER_COLOR_e.CYAN)
		{
			this.Speed = speed;
			this.Angle = angle;
			this.Width = 13.0;
		}

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			double angleAdd = 0.1;

			for (; ; )
			{
				this.Angle += angleAdd;
				angleAdd *= 0.99;

				yield return true;
			}
		}
	}
}
