using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_行き先案内 : Enemy
	{
		private bool Goal方面;

		public Enemy_行き先案内(D2Point pos, bool goal方面)
			: base(pos)
		{
			this.Goal方面 = goal方面;
		}

		public override void Draw()
		{
			const double PL_CRASH_W = GameConsts.TILE_W - 1;
			const double PL_CRASH_H = GameConsts.TILE_H - 1;

			DDCrash crash = DDCrashUtils.Rect_CenterSize(
				new D2Point(this.X, this.Y),
				new D2Size(GameConsts.TILE_W, GameConsts.TILE_H)
				);
			DDCrash plCrash = DDCrashUtils.Rect_CenterSize(
				new D2Point(Game.I.Player.X, Game.I.Player.Y),
				new D2Size(PL_CRASH_W, PL_CRASH_H)
				);

			if (crash.IsCrashed(plCrash))
			{
				if (this.Goal方面)
					Game.I.行き先案内_Crashed_Goal方面 = true;
				else
					Game.I.行き先案内_Crashed_Start方面 = true;
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_行き先案内(new D2Point(this.X, this.Y), this.Goal方面);
		}
	}
}
