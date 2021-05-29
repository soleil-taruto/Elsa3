using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Games.Designs
{
	public abstract class Design
	{
		/// <summary>
		/// 背景を描画する。
		/// </summary>
		/// <param name="cam_x">カメラの左上(X-座標)</param>
		/// <param name="cam_y">カメラの左上(Y-座標)</param>
		/// <param name="cam_xRate">カメラの位置レート(X-軸)</param>
		/// <param name="cam_yRate">カメラの位置レート(Y-軸)</param>
		public abstract void DrawWall(double cam_x, double cam_y, double cam_xRate, double cam_yRate);

		/// <summary>
		/// タイル(マップセル)を描画する。
		/// </summary>
		/// <param name="cell">マップセル</param>
		/// <param name="cell_x">マップセルの座標(X-軸_マップテーブルの座標)</param>
		/// <param name="cell_y">マップセルの座標(Y-軸_マップテーブルの座標)</param>
		/// <param name="draw_x">描画位置(X-軸_マップセルの中央)</param>
		/// <param name="draw_y">描画位置(Y-軸_マップセルの中央)</param>
		public abstract void DrawTile(MapCell cell, int cell_x, int cell_y, double draw_x, double draw_y);

		public I3Color EnemyColor_Arkanoid = new I3Color(150, 100, 200);
		public I3Color EnemyColor_Cookie = new I3Color(32, 192, 32);
		public I3Color EnemyColor_Death_A = new I3Color((int)(0.1 * 255), 0, 0);
		public I3Color EnemyColor_Death_B = new I3Color((int)(0.9 * 255), 0, 0);
		public I3Color EnemyColor_Pata = new I3Color(192, 32, 32);

		public double WallAlpha = 0.9;

		public virtual void DrawPlayer()
		{
			Game.I.Player.Draw_02();
		}
	}
}
