using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 自機狙い_へにょりレーザー
	/// </summary>
	public class Enemy_HenyoriLaser_02 : Enemy_HenyoriLaser
	{
		public Enemy_HenyoriLaser_02(double x, double y, EnemyCommon_HenyoriLaser.LASER_COLOR_e color, double angle)
			: base(x, y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.LONG, color)
		{
			this.Angle = angle;
			this.Speed = 5.0;
			this.Width = 12.0;
		}

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			Action<double> a_each = apprRate =>
			{
				double plAngle = DDUtils.GetAngle(new D2Point(Game.I.Player.X, Game.I.Player.Y) - new D2Point(this.X, this.Y));

				if (this.Angle + Math.PI < plAngle) this.Angle += Math.PI * 2;
				if (this.Angle > Math.PI + plAngle) this.Angle -= Math.PI * 2;

				DDUtils.Approach(ref this.Angle, plAngle, apprRate);
				this.Speed += 0.1;
			};

			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				a_each(0.99);
				yield return true;
			}
			foreach (DDScene scene in DDSceneUtils.Create(90))
			{
				a_each(1.0 - DDUtils.Parabola(scene.Rate) * 0.1);
				yield return true;
			}
			for (; ; )
			{
				a_each(0.9);
				yield return true;
			}
		}
	}
}
