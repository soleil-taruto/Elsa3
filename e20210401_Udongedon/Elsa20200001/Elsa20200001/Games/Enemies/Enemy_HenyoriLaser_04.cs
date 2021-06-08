using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 自機狙い_へにょりレーザー_v2
	/// </summary>
	public class Enemy_HenyoriLaser_04 : Enemy_HenyoriLaser
	{
		public Enemy_HenyoriLaser_04(double x, double y, EnemyCommon_HenyoriLaser.LASER_COLOR_e color)
			: base(x, y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.SHORT, color)
		{
			this.Angle = Math.PI * 1.5;
			this.Speed = 20.0;
			this.Width = 13.0;
		}

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			Action<double> a_each = apprRate =>
			{
				double plAngle = DDUtils.GetAngle(new D2Point(Game.I.Player.X, Game.I.Player.Y) - new D2Point(this.X, this.Y));

				if (this.Angle + Math.PI < plAngle) this.Angle += Math.PI * 2;
				if (this.Angle > Math.PI + plAngle) this.Angle -= Math.PI * 2;

				DDUtils.Approach(ref this.Angle, plAngle, apprRate);
			};

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				a_each(1.0 - DDUtils.Parabola(scene.Rate));
				this.Speed *= 0.93;
				yield return true;
			}
			for (; ; )
			{
				a_each(0.99);
				this.Speed += 2.0;
				yield return true;
			}
		}
	}
}
