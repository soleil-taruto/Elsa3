﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_Laser : Shot
	{
		private int Level; // 0 ～ Consts.PLAYER_LEVEL_MAX

		public Shot_Laser(double x, double y, int level)
			: base(x, y, Kind_e.NORMAL, new int[] { 3, 6, 7, 13, 15, 21 }[level])
		{
			this.Level = level;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (int frame = 0; ; frame++)
			{
				this.Y -= 18.0; // 速い

				if (this.Y < 0.0)
					break;

				DDDraw.SetAlpha(ShotConsts.A);
				DDDraw.DrawBegin(Ground.I.Picture2.D_LASER, this.X, this.Y);
				DDDraw.DrawZoom(1.0 + (0.5 * this.Level) / GameConsts.PLAYER_LEVEL_MAX);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(
					new D2Point(this.X, this.Y),
					32.0 + (16.0 * this.Level) / GameConsts.PLAYER_LEVEL_MAX
					);

				yield return true;
			}
		}
	}
}
