using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Designs
{
	public class Design_0002 : Design
	{
		public override void DrawWall(double cam_x, double cam_y, double cam_xRate, double cam_yRate)
		{
			DDDraw.SetBright(new I3Color(255, 0, 0));
			DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
			DDDraw.Reset();
		}

		public override void DrawTile(MapCell cell, int cell_x, int cell_y, double draw_x, double draw_y)
		{
			if (cell.Kind == MapCell.Kind_e.WALL)
			{
				double bure_x = Math.Sin((DDEngine.ProcFrame + cell_x + cell_y) * 0.05) * 8.0;
				double bure_y = Math.Sin((DDEngine.ProcFrame + cell_x + cell_y) * 0.07) * 8.0;

				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(new I3Color(0, 0, 0));
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
	}
}
