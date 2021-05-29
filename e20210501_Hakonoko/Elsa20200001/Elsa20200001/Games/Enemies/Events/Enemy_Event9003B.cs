using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Designs;

namespace Charlotte.Games.Enemies.Events
{
	public class Enemy_Event9003B : Enemy
	{
		public Enemy_Event9003B(D2Point pos)
			: base(pos)
		{ }

		public override void Draw()
		{
			if (DDUtils.GetDistance(new D2Point(this.X, this.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) < 50.0)
			{
				if (Game.I.FinalZone.OH_Event9003B.Once())
				{
					Game.I.Map.Design = new Design_0003();
					DDGround.EL.Add(SCommon.Supplier(Effects.Liteフラッシュ()));
					//DDGround.EL.Add(SCommon.Supplier(Effects.Heavyフラッシュ()));

					Ground.I.Music.地鳴り.Play(false, false, 0.1, 10);
				}
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Event9003B(new D2Point(this.X, this.Y));
		}
	}
}
