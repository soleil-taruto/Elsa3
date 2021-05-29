using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Events
{
	public class Enemy_Event9002 : Enemy
	{
		private int 発生領域;

		public Enemy_Event9002(D2Point pos, int 発生領域)
			: base(pos)
		{
			this.発生領域 = 発生領域;
		}

		public override void Draw()
		{
			if (DDUtils.GetDistance(new D2Point(this.X, this.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) < 50.0)
			{
				Enemy_MeteorLoader meteorLoader = (Enemy_MeteorLoader)Game.I.Enemies.Iterate().First(enemy => enemy is Enemy_MeteorLoader);

				meteorLoader.発生領域 = this.発生領域;
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Event9002(new D2Point(this.X, this.Y), this.発生領域);
		}
	}
}
