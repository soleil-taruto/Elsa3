using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Designs
{
	public class Design_0003 : Design
	{
		public override void DrawWall(double cam_x, double cam_y, double cam_xRate, double cam_yRate)
		{
			DDDraw.SetBright(new I3Color(0, 0, 20));
			DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
			DDDraw.Reset();

			if (DDEngine.ProcFrame % 90 == 0)
			{
				波紋効果.Addマップ下部から炎();
			}

			DDGround.EL.Add(() =>
			{
				double map_b = Game.I.Map.H * GameConsts.TILE_H;
				double cam_b = DDGround.Camera.Y + DDConsts.Screen_H;

				double y = map_b - cam_b;
				y *= 0.5;
				y = DDConsts.Screen_H - Ground.I.Picture.WallFire.Get_H() + y;

				DDDraw.SetBright(new I3Color(255, 0, 0));
				DDDraw.DrawSimple(Ground.I.Picture.WallFire, 0, y);
				DDDraw.Reset();

				if (DDEngine.ProcFrame % 20 == 0)
				{
					double yy = map_b - cam_b;
					yy /= 2000.0;
					yy = 1.0 - yy;
					DDUtils.ToRange(ref yy, 0.0, 1.0);
					yy = 0.3 + yy * 0.7;

					// yy == 0.3 ～ 1.0 == 高い位置 ～ 低い位置

					if (DDConfig.LOG_ENABLED)
					{
						DDGround.EL.Keep(10, () =>
						{
							DDPrint.SetPrint(0, 16);
							DDPrint.DebugPrint("yy: " + yy);
						});
					}
					DDMusicUtils.Fade(10, yy);
				}
				return false;
			});
		}

		public override void DrawTile(MapCell cell, int cell_x, int cell_y, double draw_x, double draw_y)
		{
			if (cell.Kind == MapCell.Kind_e.WALL)
			{
				double bure_x = Math.Sin((DDEngine.ProcFrame + cell_x + cell_y) * 0.013) * 10.0;
				double bure_y = Math.Sin((DDEngine.ProcFrame + cell_x + cell_y) * 0.017) * 10.0;

				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(new I3Color(255, 255, 255));
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, draw_x, draw_y);
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawSlide(
					DDUtils.Random.DReal() * bure_x,
					DDUtils.Random.DReal() * bure_y
					);
				DDDraw.DrawEnd();
				DDDraw.Reset();
			}
		}

		public override void DrawPlayer()
		{
			double map_b = Game.I.Map.H * GameConsts.TILE_H;
			double pl_y = Game.I.Player.Y;

			double y = (map_b - 70) - pl_y;
			y /= 300.0;
			y = Math.Abs(y);

			if (y < 1.0)
			{
				DDDraw.SetAlpha(y);
			}
			DDDraw.DrawBegin(
				Ground.I.Picture.WhiteBox,
				SCommon.ToInt(Game.I.Player.X - DDGround.ICamera.X),
				SCommon.ToInt(Game.I.Player.Y - DDGround.ICamera.Y)
				);
			DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}
	}
}
