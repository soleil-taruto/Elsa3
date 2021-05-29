using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Events
{
	public class Enemy_Event9005 : Enemy
	{
		public Enemy_Event9005(D2Point pos)
			: base(pos)
		{ }

		public override void Draw()
		{
			if (DDUtils.GetDistance(new D2Point(this.X, this.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) < 50.0)
			{
				Game.I.FinalZone.Ending_復讐_突入 = true;
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Event9005(new D2Point(this.X, this.Y));
		}
	}
}

