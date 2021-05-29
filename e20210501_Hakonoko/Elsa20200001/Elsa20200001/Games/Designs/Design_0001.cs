using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;

namespace Charlotte.Games.Designs
{
	public class Design_0001 : Design
	{
		// ---- set by Ctor ----

		private DDPicture WallPicture_01;
		private DDPicture WallPicture_02;
		private DDPicture WallPicture_03;
		public I3Color Color_01;
		public I3Color Color_02;
		public I3Color Color_03;
		public I3Color Color_A;
		public I3Color Color_B;
		private bool 市松tic;

		// ---- set by extendedCtor ----

		public I3Color CookieAxisColor = new I3Color(200, 230, 255);

		// ----

		public Design_0001(
			DDPicture wallPicture_01,
			DDPicture wallPicture_02,
			DDPicture wallPicture_03,
			I3Color color_01,
			I3Color color_02,
			I3Color color_03,
			I3Color color_a,
			I3Color color_b,
			I3Color enemyColor_arkanoid,
			I3Color enemyColor_cookie,
			I3Color enemyColor_death_a,
			I3Color enemyColor_death_b,
			I3Color enemyColor_pata,
			double wallAlpha,
			bool 市松tic,
			Action<Design_0001> extendedCtor
			)
		{
			this.WallPicture_01 = wallPicture_01;
			this.WallPicture_02 = wallPicture_02;
			this.WallPicture_03 = wallPicture_03;
			this.Color_01 = color_01;
			this.Color_02 = color_02;
			this.Color_03 = color_03;
			this.Color_A = color_a;
			this.Color_B = color_b;
			this.EnemyColor_Arkanoid = enemyColor_arkanoid;
			this.EnemyColor_Cookie = enemyColor_cookie;
			this.EnemyColor_Death_A = enemyColor_death_a;
			this.EnemyColor_Death_B = enemyColor_death_b;
			this.EnemyColor_Pata = enemyColor_pata;
			this.WallAlpha = wallAlpha;
			this.市松tic = 市松tic;
		}

		public override void DrawWall(double cam_x, double cam_y, double cam_xRate, double cam_yRate)
		{
			this.DrawWall(this.WallPicture_01, this.Color_01, cam_xRate, cam_yRate);
			this.DrawWall(this.WallPicture_02, this.Color_02, cam_xRate, cam_yRate);
			this.DrawWall(this.WallPicture_03, this.Color_03, cam_xRate, cam_yRate);

			if (Game.I.FinalZone != null)
			{
				DDCurtain.DrawCurtain(Game.I.FinalZone.Rate * -0.5);
			}
		}

		private void DrawWall(DDPicture picture, I3Color color, double xRate, double yRate)
		{
			int xSpan = picture.Get_W() - DDConsts.Screen_W;
			int ySpan = picture.Get_H() - DDConsts.Screen_H;
			int span = Math.Min(xSpan, ySpan);

			DDDraw.SetBright(color);
			DDDraw.DrawCenter(
				picture,
				DDConsts.Screen_W / 2 - (xRate - 0.5) * span,
				DDConsts.Screen_H / 2 - (yRate - 0.5) * span
				);
			DDDraw.Reset();
		}

		public override void DrawTile(MapCell cell, int cell_x, int cell_y, double draw_x, double draw_y)
		{
			if (
				cell.Kind == MapCell.Kind_e.WALL ||
				cell.Kind == MapCell.Kind_e.WALL_ENEMY_THROUGH ||
				cell.IsCookie()
				)
			{
				// cell.敵接近_Rate 更新
				{
					double rate = GetEnemyNearlyRate(
						DDGround.Camera.X + draw_x,
						DDGround.Camera.Y + draw_y
						);

					DDUtils.Maxim(ref cell.敵接近_Rate, rate);

					cell.敵接近_Rate *= 0.97;
				}

				double p = cell.ColorPhase;

				if (this.市松tic)
				{
					if ((cell_x + cell_y) % 2 == 0)
						p = p * 0.7;
					else
						p = p * 0.3 + 0.7;
				}

				if (cell.ColorPhaseShift < -SCommon.MICRO)
					p *= cell.ColorPhaseShift + 1.0;
				else if (SCommon.MICRO < cell.ColorPhaseShift)
					p = DDUtils.AToBRate(p, 1.0, cell.ColorPhaseShift);

				double a;

				if (cell.Kind == MapCell.Kind_e.WALL_ENEMY_THROUGH)
					a = this.WallAlpha * Math.Pow(1.0 - cell.敵接近_Rate, 2.0);
				else
					a = this.WallAlpha * (1.0 - cell.敵接近_Rate * 0.5);

				DDDraw.SetAlpha(a);
				DDDraw.SetBright(new I3Color(
					SCommon.ToInt(DDUtils.AToBRate(this.Color_A.R, this.Color_B.R, p)),
					SCommon.ToInt(DDUtils.AToBRate(this.Color_A.G, this.Color_B.G, p)),
					SCommon.ToInt(DDUtils.AToBRate(this.Color_A.B, this.Color_B.B, p))
					));
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, draw_x, draw_y);
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				if (cell.IsCookie())
				{
					DDDraw.SetBright(this.CookieAxisColor);
					DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, draw_x, draw_y);
					DDDraw.DrawSetSize(GameConsts.TILE_W / 2, GameConsts.TILE_H / 2);
					DDDraw.DrawEnd();
					DDDraw.Reset();
				}
			}
			else if (cell.Kind == MapCell.Kind_e.GOAL)
			{
				//double bright =
				//    Math.Sin(DDEngine.ProcFrame / 37.0) * 0.3 +
				//    Math.Sin(DDEngine.ProcFrame / 0.7) * 0.1 +
				//    0.6;
				double bright = Math.Sin(DDEngine.ProcFrame / 13.0) * 0.4 + 0.6;

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

		private static double GetEnemyNearlyRate(double x, double y)
		{
			//if (Game.I.Enemies.Count == 0)
			//    return 0.0;

			double nearestDistance = SCommon.IMAX;

			foreach (D2Point pt in Game.I.タイル接近_敵描画_Points.Iterate())
			{
				double distance = DDUtils.GetDistance(new D2Point(pt.X, pt.Y), new D2Point(x, y));

				if (distance < nearestDistance)
					nearestDistance = distance;
			}

			const double RADIUS = 70.0;

			double rate = 1.0 - nearestDistance / RADIUS;
			DDUtils.ToRange(ref rate, 0.0, 1.0);

			return rate;
		}
	}
}
