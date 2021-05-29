using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_MeteorLoader : Enemy
	{
		private DDRandom Random = new DDRandom(1u);

		public Enemy_MeteorLoader(D2Point pos)
			: base(pos)
		{ }

		public int 発生領域 = 5; // { 2, 4, 5, 6, 8 } == 下, 左, 全域, 右, 上

		private int Frame = 0;

		public override void Draw()
		{
			if (Game.I.Player.DeadFrame != 0) // ? プレイヤー死亡
			{
				int addcntmax = 1 + (Game.I.Player.DeadFrame * 5) / GameConsts.PLAYER_DEAD_FRAME_MAX;

				for (int addcnt = 0; addcnt < addcntmax; addcnt++)
				{
					D2Point enemyPos = new D2Point(
						DDGround.Camera.X - DDConsts.Screen_W / 2 + DDConsts.Screen_W * DDUtils.Random.Real() * 2,
						DDGround.Camera.Y - DDConsts.Screen_H / 2 + DDConsts.Screen_H * DDUtils.Random.Real() * 2
						);

					//Game.I.Enemies.Add(new Enemy_Death(enemyPos));
					Game.I.Enemies.Add(new Enemy_Meteor(enemyPos));
				}
			}
			//else if (20 < this.Frame && this.Random.Real() < Math.Min(0.2, this.Frame / 500.0))
			else if (
				20 < this.Frame &&
				this.Random.GetInt(Math.Max(5, 100 - this.Frame)) == 0 // Frame == 0.0 -> NaN 注意
				)
			{
				for (int retry = 0; retry < 10; retry++)
				{
					D2Point dotPos;

					switch (this.発生領域)
					{
						case 2:
							dotPos = new D2Point(
								DDGround.ICamera.X + this.Random.GetInt(DDConsts.Screen_W),
								DDGround.ICamera.Y + this.Random.GetInt(DDConsts.Screen_H / 2) + DDConsts.Screen_H / 2
								);
							break;

						case 4:
							dotPos = new D2Point(
								DDGround.ICamera.X + this.Random.GetInt(DDConsts.Screen_W / 2),
								DDGround.ICamera.Y + this.Random.GetInt(DDConsts.Screen_H)
								);
							break;

						case 5:
							dotPos = new D2Point(
								DDGround.ICamera.X + this.Random.GetInt(DDConsts.Screen_W),
								DDGround.ICamera.Y + this.Random.GetInt(DDConsts.Screen_H)
								);
							break;

						case 6:
							dotPos = new D2Point(
								DDGround.ICamera.X + this.Random.GetInt(DDConsts.Screen_W / 2) + DDConsts.Screen_W / 2,
								DDGround.ICamera.Y + this.Random.GetInt(DDConsts.Screen_H)
								);
							break;

						case 8:
							dotPos = new D2Point(
								DDGround.ICamera.X + this.Random.GetInt(DDConsts.Screen_W),
								DDGround.ICamera.Y + this.Random.GetInt(DDConsts.Screen_H / 2)
								);
							break;

						default:
							throw null; // never
					}

					I2Point cellPos = GameCommon.ToTablePoint(dotPos);
					MapCell cell = Game.I.Map.GetCell(cellPos);

					if (
						!cell.IsDefault && // ? 画面外ではない。
						300.0 < DDUtils.GetDistance(new D2Point(dotPos.X, dotPos.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) && // ? 近すぎない。
						(cell.Kind == MapCell.Kind_e.WALL || cell.Kind == MapCell.Kind_e.DEATH)
						)
					{
						if (cell.Kind == MapCell.Kind_e.WALL)
						{
							cell.Kind = MapCell.Kind_e.DEATH;
							cell.KindOrig = MapCell.Kind_e.WALL;
						}

						D2Point enemyPos = new D2Point(
							cellPos.X * GameConsts.TILE_W + GameConsts.TILE_W / 2,
							cellPos.Y * GameConsts.TILE_H + GameConsts.TILE_H / 2
							);

						Game.I.Enemies.Add(new Enemy_Death(enemyPos));
						Game.I.Enemies.Add(new Enemy_Meteor(enemyPos));

						break;
					}
				}
			}
			this.Frame++;
		}

		public override Enemy GetClone()
		{
			return new Enemy_MeteorLoader(new D2Point(this.X, this.Y));
		}
	}
}
