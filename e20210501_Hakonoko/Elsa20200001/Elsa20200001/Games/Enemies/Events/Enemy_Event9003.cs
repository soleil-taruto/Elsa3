using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Designs;

namespace Charlotte.Games.Enemies.Events
{
	public class Enemy_Event9003 : Enemy
	{
		public Enemy_Event9003(D2Point pos)
			: base(pos)
		{ }

		public override void Draw()
		{
			if (DDUtils.GetDistance(new D2Point(this.X, this.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) < 50.0)
			{
				if (Game.I.FinalZone.OH_Event9003.Once())
				{
					for (int x = 0; x < Game.I.Map.W; x++)
					{
						for (int y = 0; y < Game.I.Map.H; y++)
						{
							MapCell cell = Game.I.Map.GetCell(x, y);

							if (
								cell.Kind == MapCell.Kind_e.DEATH ||
								cell.Kind == MapCell.Kind_e.GOAL
								)
								cell.Kind = MapCell.Kind_e.WALL;
						}
					}

					// 必要なイベント以外を除去
					//
					Game.I.Enemies.RemoveAll(enemy => !(
						enemy is Enemy_Event9003B ||
						enemy is Enemy_Event9004 ||
						enemy is Enemy_Event9005
						));

					Game.I.Map.Design = new Design_0002();

					//DDGround.EL.Add(SCommon.Supplier(Effects.Liteフラッシュ()));
					DDGround.EL.Add(SCommon.Supplier(Effects.Heavyフラッシュ()));

					DDMusicUtils.Stop();
				}
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Event9003(new D2Point(this.X, this.Y));
		}
	}
}
