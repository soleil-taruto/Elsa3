using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.チルノs
{
	public class Enemy_チルノ_HenyoriLaser_01 : Enemy_HenyoriLaser
	{
		private double[] AngleAdds;

		public Enemy_チルノ_HenyoriLaser_01(double x, double y, double speed, double angle, double[] angleAdds, EnemyCommon_HenyoriLaser.LASER_COLOR_e color)
			: base(x, y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.LONG, color)
		{
			this.Speed = speed;
			this.Angle = angle;
			this.Width = 13.0;

			this.AngleAdds = angleAdds;
		}

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			for (int c = 0; c < 5; c++)
			{
				foreach (double const_angleAdd in this.AngleAdds)
				{
					double angleAdd = const_angleAdd;

					foreach (DDScene scene in DDSceneUtils.Create(30))
					{
						this.Angle += angleAdd;
						angleAdd *= 0.99;

						yield return true;
					}
				}
			}
		}
	}
}
