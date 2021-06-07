using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_HenyoriLaser_01 : Enemy_HenyoriLaser
	{
		public Enemy_HenyoriLaser_01(double x, double y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e lenKind, EnemyCommon_HenyoriLaser.LASER_COLOR_e color)
			: base(x, y, lenKind, color)
		{ }

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			this.Angle = DDUtils.Random.Real() * Math.PI * 2.0;
			this.Speed = DDUtils.Random.Real() * 10.0 + 5.0;
			this.Width = DDUtils.Random.Real() * 15.0 + 5.0;

			double ad = this.Angle + DDUtils.Random.DReal() * Math.PI * 2.0;
			double ar = DDUtils.Random.Real() * 0.098 + 0.901;
			double sd = DDUtils.Random.Real() * 10.0 + 5.0;
			double sr = DDUtils.Random.Real() * 0.098 + 0.901;
			double wd = DDUtils.Random.Real() * 15.0 + 5.0;
			double wr = DDUtils.Random.Real() * 0.098 + 0.901;

			for (; ; )
			{
				DDUtils.Approach(ref this.Angle, ad, ar);
				DDUtils.Approach(ref this.Speed, sd, sr);
				DDUtils.Approach(ref this.Width, wd, wr);

				yield return true;
			}
		}
	}
}
