using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Designs
{
	public class Design_0000 : Design
	{
		public override void DrawWall(double cam_x, double cam_y, double cam_xRate, double cam_yRate)
		{
			// HACK: 重い！

			const int WALL_TILE_WH = 28;

			int l = -(int)(cam_x / 10.0);
			int t = -(int)(cam_y / 10.0);

			for (int x = 0; l + x * WALL_TILE_WH < DDConsts.Screen_W; x++)
			{
				int draw_l = l + x * WALL_TILE_WH;
				int draw_r = draw_l + WALL_TILE_WH;

				if (draw_r <= 0)
					continue;

				for (int y = 0; t + y * WALL_TILE_WH < DDConsts.Screen_H; y++)
				{
					int draw_t = t + y * WALL_TILE_WH;
					int draw_b = draw_t + WALL_TILE_WH;

					if (draw_b <= 0)
						continue;

					//DX.DrawBox(draw_l, draw_t, draw_r, draw_b, (x + y) % 2 == 0 ? DX.GetColor(0, 0, 32) : DX.GetColor(32, 32, 64), 1);

					if ((x + y) % 2 == 0)
						DDDraw.SetBright(new I3Color(32, 32, 48));
					else
						DDDraw.SetBright(new I3Color(48, 48, 64));

					DDDraw.DrawBeginRect_LTRB(Ground.I.Picture.WhiteBox, draw_l, draw_t, draw_r, draw_b);
					DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
					DDDraw.DrawEnd();
					DDDraw.Reset();
				}
			}
		}

		public override void DrawTile(MapCell cell, int cell_x, int cell_y, double draw_x, double draw_y)
		{
			if (
				cell.Kind == MapCell.Kind_e.WALL ||
				cell.Kind == MapCell.Kind_e.WALL_ENEMY_THROUGH ||
				cell.IsCookie()
				)
			{
				if ((cell_x + cell_y) % 2 == 0)
					DDDraw.SetBright(new I3Color(128, 192, 255));
				else
					DDDraw.SetBright(new I3Color(100, 150, 200));

				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, draw_x, draw_y);
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				if (cell.IsCookie())
				{
					DDDraw.SetBright(new I3Color(200, 230, 255));
					DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, draw_x, draw_y);
					DDDraw.DrawSetSize(GameConsts.TILE_W / 2, GameConsts.TILE_H / 2);
					DDDraw.DrawEnd();
					DDDraw.Reset();
				}
			}
			else if (cell.Kind == MapCell.Kind_e.GOAL)
			{
				double bright = Math.Sin(DDEngine.ProcFrame / 10.0) * 0.4 + 0.6;

				DDDraw.SetBright(
					bright * 0.5,
					bright * 0.9,
					bright * 1.0
					);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, draw_x, draw_y);
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawEnd();
				DDDraw.Reset();
			}
		}
	}
}
