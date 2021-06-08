using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 放射用_へにょりレーザー
	/// </summary>
	public class Enemy_HenyoriLaser_03 : Enemy_HenyoriLaser
	{
		public Enemy_HenyoriLaser_03(double x, double y, EnemyCommon_HenyoriLaser.LASER_COLOR_e color, double angle)
			: base(x, y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.MIDDLE, color)
		{
			this.Angle = angle;
			this.Speed = 5.0;
			this.Width = 20.0;
		}

		protected override IEnumerable<bool> E_UpdateParameters()
		{
			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				this.Angle += DDUtils.Parabola(scene.Rate) * 0.8;
				yield return true;
			}
			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.Angle += DDUtils.Parabola(scene.Rate) * -0.01;
				this.Speed += 0.1;
				yield return true;
			}
			foreach (DDScene scene in DDSceneUtils.Create(70))
			{
				this.Angle += 0.1;
				this.Speed += 0.02;
				yield return true;
			}
			for (; ; )
			{
				this.Angle += -0.01;
				yield return true;
			}
		}
	}
}
