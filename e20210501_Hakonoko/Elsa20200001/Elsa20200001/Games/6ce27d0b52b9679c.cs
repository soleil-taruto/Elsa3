using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class 波紋効果
	{
		// HACK: 謎の重さがある。

		private const int PIECE_WH = 60;
		private const int PIECES_W = 16;
		private const int PIECES_H = 9;

		private static DDSubScreen Screen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
		private static DDSubScreen[,] PieceTable = new DDSubScreen[PIECES_W, PIECES_H];

		public static void INIT()
		{
			for (int x = 0; x < PIECES_W; x++)
				for (int y = 0; y < PIECES_H; y++)
					PieceTable[x, y] = new DDSubScreen(PIECE_WH, PIECE_WH);
		}

		private static DDTaskList 波紋s = new DDTaskList();

		public static void Clear()
		{
			波紋s.Clear();
		}

		public static void Add(double x, double y)
		{
			if (10 <= 波紋s.Count)
			{
				波紋s.RemoveAt(0);
			}
			波紋s.Add(new 波紋Task(x, y).Task);
		}

		public static void Addマップ下部から炎()
		{
			波紋s.Add(new マップ下部から炎Task().Task);
		}

		public static int Count
		{
			get
			{
				return 波紋s.Count;
			}
		}

		private static D2Point[,] PointTable = new D2Point[PIECES_W + 1, PIECES_H + 1];

		private class 波紋Task : DDTask
		{
			private double Center_X;
			private double Center_Y;

			public 波紋Task(double x, double y)
			{
				this.Center_X = x;
				this.Center_Y = y;
			}

			public override IEnumerable<bool> E_Task()
			{
				foreach (DDScene scene in DDSceneUtils.Create(600))
				{
					for (int x = 0; x <= PIECES_W; x++)
						for (int y = 0; y <= PIECES_H; y++)
							PointTable[x, y] = this.波紋効果による頂点の移動(PointTable[x, y], scene.Rate);

					yield return true;
				}
			}

			private D2Point 波紋効果による頂点の移動(D2Point pt, double rate)
			{
				// pt == 画面上の座標

				// マップ上の座標に変更
				pt.X += DDGround.ICamera.X;
				pt.Y += DDGround.ICamera.Y;

				// 波紋の中心からの相対座標に変更
				pt.X -= this.Center_X;
				pt.Y -= this.Center_Y;

				//double wave_r = rate * 6000.0;
				double wave_r = rate * 5000.0 + rate * rate * 1000.0;
				double distance = DDUtils.GetDistance(pt);
				double d = distance;

				d -= wave_r;

				// d の -(50 + a) ～ (50 + a) を 0.0 ～ 1.0 にする。
				d /= 50.0;
				//d /= 50.0 + rate * 50.0;
				//d /= 50.0 + rate * 100.0; // ガクガクする。
				//d /= 50.0 + rate * 150.0; // ガクガクする。
				d += 1.0;
				d /= 2.0;

				if (0.0 < d && d < 1.0)
				{
					d *= 2.0;

					if (1.0 < d)
						d = 2.0 - d;

					distance += DDUtils.SCurve(d) * 50.0;
					//distance += DDUtils.SCurve(d) * 50.0 * (1.0 - rate * 0.5); // 弱めるとガクガクする。

					DDUtils.MakeXYSpeed(0.0, 0.0, pt.X, pt.Y, distance, out pt.X, out pt.Y); // distance を pt に反映する。
				}

				// restore -- 波紋の中心からの相対座標 -> マップ上の座標
				pt.X += this.Center_X;
				pt.Y += this.Center_Y;

				// restore -- マップ上の座標 -> 画面上の座標
				pt.X -= DDGround.ICamera.X;
				pt.Y -= DDGround.ICamera.Y;

				return pt;
			}
		}

		private class マップ下部から炎Task : DDTask
		{
			private double Power定数 = DDUtils.Random.Real();

			public override IEnumerable<bool> E_Task()
			{
				foreach (DDScene scene in DDSceneUtils.Create(800))
				{
					for (int x = 0; x <= PIECES_W; x++)
						for (int y = 0; y <= PIECES_H; y++)
							PointTable[x, y] = this.ChangePoint(PointTable[x, y], scene.Rate);

					yield return true;
				}
			}

			private D2Point ChangePoint(D2Point pt, double rate)
			{
				const double WAVE_SPAN = 50.0;

				// マップ上のY座標
				double mapY = DDGround.Camera.Y + pt.Y;

				// 波の頂点Y座標
				double waveY = Game.I.Map.H * GameConsts.TILE_H + WAVE_SPAN - rate * 1400.0;

				double waveYDiff = mapY - waveY;
				waveYDiff /= WAVE_SPAN;
				waveYDiff = Math.Abs(waveYDiff);

				if (waveYDiff < 1.0)
				{
					double power = Math.Sin((this.Power定数 + pt.X / DDConsts.Screen_W) * Math.PI * 2.0) + 1.0;
					double shiftY = DDUtils.SCurve(1.0 - waveYDiff) * (1.0 - rate) * 30.0 * power;

					pt.Y += shiftY;
				}
				return pt;
			}
		}

		public static bool 抑止 = false;

		public static void EachFrame()
		{
			if (抑止)
				return;

			if (波紋s.Count == 0)
				return;

			for (int x = 0; x <= PIECES_W; x++)
				for (int y = 0; y <= PIECES_H; y++)
					PointTable[x, y] = new D2Point(x * PIECE_WH, y * PIECE_WH);

			波紋s.ExecuteAllTask();

			for (int x = 0; x < PIECES_W; x++)
			{
				for (int y = 0; y < PIECES_H; y++)
				{
					PieceTable[x, y].ChangeDrawScreen();

					DX.DrawRectGraph(0, 0, x * PIECE_WH, y * PIECE_WH, (x + 1) * PIECE_WH, (y + 1) * PIECE_WH, DDGround.MainScreen.GetHandle(), 0);
				}
			}

			Screen.ChangeDrawScreen();

			for (int x = 0; x < PIECES_W; x++)
			{
				for (int y = 0; y < PIECES_H; y++)
				{
					D2Point lt = PointTable[x + 0, y + 0];
					D2Point rt = PointTable[x + 1, y + 0];
					D2Point rb = PointTable[x + 1, y + 1];
					D2Point lb = PointTable[x + 0, y + 1];

					DDDraw.SetIgnoreError();
					DDDraw.DrawFree(PieceTable[x, y].ToPicture(), lt, rt, rb, lb);
					DDDraw.Reset();
				}
			}

			DDGround.MainScreen.ChangeDrawScreen();

			DDDraw.DrawSimple(Screen.ToPicture(), 0, 0);
		}
	}
}
