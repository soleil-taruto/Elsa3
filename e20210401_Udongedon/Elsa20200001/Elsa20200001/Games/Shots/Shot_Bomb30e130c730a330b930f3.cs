using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_Bombメディスン : Shot
	{
		public Shot_Bombメディスン(double x, double y)
			: base(x, y, Kind_e.BOMB, 10000)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			RippleEffect.Add_波紋(this.X, this.Y, 60);
			RippleEffect.Add_波紋(this.X, this.Y, 120);
			RippleEffect.Add_波紋(this.X, this.Y, 180);
			RippleEffect.Add_波紋(this.X, this.Y, 360);

			DDGround.EL.Add(SCommon.Supplier(this.マスク効果()));

			this.マスク_Color.A = 0.75;
			this.マスク_TargetColor.G = 0.0;
			this.マスク_TargetColor.A = 0.25;

			for (int frame = 0; frame < GameConsts.PLAYER_BOMB_FRAME_MAX + 60; frame++)
			{
				this.Crash = DDCrashUtils.Rect(D4Rect.LTRB(
					0,
					0,
					GameConsts.FIELD_W,
					GameConsts.FIELD_H
					));

				yield return true;
			}

			this.マスク_TargetColor.A = 0.0;
		}

		private D4Color マスク_Color = new D4Color(1.0, 1.0, 1.0, 1.0);
		private D4Color マスク_TargetColor = new D4Color(1.0, 1.0, 1.0, 1.0);

		private IEnumerable<bool> マスク効果()
		{
			while (SCommon.MICRO < this.マスク_Color.A)
			{
				const double APPROACH_RATE = 0.97;

				DDUtils.Approach(ref this.マスク_Color.R, this.マスク_TargetColor.R, APPROACH_RATE);
				DDUtils.Approach(ref this.マスク_Color.G, this.マスク_TargetColor.G, APPROACH_RATE);
				DDUtils.Approach(ref this.マスク_Color.B, this.マスク_TargetColor.B, APPROACH_RATE);
				DDUtils.Approach(ref this.マスク_Color.A, this.マスク_TargetColor.A, APPROACH_RATE);

				DDDraw.SetAlpha(this.マスク_Color.A);
				DDDraw.SetBright(this.マスク_Color.R, this.マスク_Color.G, this.マスク_Color.B);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
