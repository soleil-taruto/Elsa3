using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class 自機位置調整
	{
		private static Predicate<MapCell> IsWall;
		private static int IX;
		private static int IY;
		private static Around A2;
		private static Around A3;

		public enum Touch_e
		{
			AIRBORNE = 0,
			AIRBORNE_Y_TURNED,
			GROUND,
			ROOF,
		}

		public static Touch_e Touch;

		public static void Perform(ref double x, ref double y, Predicate<MapCell> isWall)
		{
			IsWall = isWall;

			// 整数化
			IX = SCommon.ToInt(x);
			IY = SCommon.ToInt(y);

			A2 = new Around(IX, IY, 2);
			A3 = new Around(IX, IY, 3);

			Touch = Touch_e.AIRBORNE;

			I2Point a2RelPtBk = A2.RelativePoint;

			Perform_A(false, false);

			if (
				a2RelPtBk.X != A2.RelativePoint.X ||
				a2RelPtBk.Y != A2.RelativePoint.Y
				)
			{
				x = A2.CenterPoint.X + A2.RelativePoint.X;
				y = A2.CenterPoint.Y + A2.RelativePoint.Y;
			}
		}

		private static void Perform_A(bool xTurned, bool yTurned)
		{
			switch (
				(IsWall(A2.Table[0, 1]) ? 1 : 0) | // 左下
				(IsWall(A2.Table[1, 1]) ? 2 : 0) | // 右下
				(IsWall(A2.Table[0, 0]) ? 4 : 0) | // 左上
				(IsWall(A2.Table[1, 0]) ? 8 : 0)   // 右上
				)
			{
				case 0: // 壁なし
					break;

				case 15: // 壁の中
					for (int x = 0; x < 3; x++)
					{
						for (int y = 0; y < 3; y++)
						{
							if (!A3.Table[x, y].IsWall())
							{
								A2.RelativePoint.X += (x - 1) * 10;
								A2.RelativePoint.Y += (y - 1) * 10;

								goto endInsideWall;
							}
						}
					}
					A2.RelativePoint.X += DDUtils.Random.ChooseOne(new int[] { -10, 10 });
					A2.RelativePoint.Y += DDUtils.Random.ChooseOne(new int[] { -10, 10 });

				endInsideWall:
					break;

				case 1: // 左下のみ
				case 4: // 左上のみ
				case 5: // 左
				case 7: // 右上のみ空き
				case 13: // 右下のみ空き
					A2.XTurn();
					Perform_A(true, yTurned);
					A2.RelativePoint.X *= -1;
					break;

				case 8: // 右上のみ
				case 9: // 右上と左下
				case 12: // 上
				case 14: // 左下のみ空き
					A2.YTurn();
					Perform_A(xTurned, true);
					A2.RelativePoint.Y *= -1;
					Touch = (Touch_e)((int)Touch ^ 1);
					break;

				case 2: // 右下のみ
					if (-16 < A2.RelativePoint.X && -16 < A2.RelativePoint.Y)
					{
						if (Game.I.Player.YSpeed < 0.0 || yTurned) // ? 上昇中 || 天井に当たった。
						{
							bool 強制上昇 = 1 <= DDInput.A.GetInput();

							if (A2.RelativePoint.X < (強制上昇 ? (-16 + 20) : A2.RelativePoint.Y))
							{
								A2.RelativePoint.X = -16;
							}
							else
							{
								A2.RelativePoint.Y = -16;
								Touch = Touch_e.GROUND; // 天井に当たった場合 YTurn しているので、ここは GROUND で良い。
							}
						}
						else // ? 接地中 || 落下中
						{
							// 接地中でも Game.I.Player.YSpeed に自由落下速度が加算されている。なので 0.0 ではないことに注意

							bool 強制落下 = 1 <= DDInput.DIR_2.GetInput() && DDInput.L.GetInput() <= 0; // ? 下ボタン押下 && 画面スライド中ではない。

							//Common.DebugPrint(A2.RelativePoint.X); // test

							if (強制落下)
							{
								if (A2.RelativePoint.X < -8)
								{
									A2.RelativePoint.X = -16;
								}
								else
								{
									if (A2.RelativePoint.X < 16)
										A2.RelativePoint.X--;

									A2.RelativePoint.Y = -16;
									Touch = Touch_e.GROUND;
								}
							}
							else
							{
								// 落下最高速度 == 7.0 --> 7.1ピクセル以上入り込んでくることは無いはず。--> -16 + 7.1 == -8.9

								if (A2.RelativePoint.Y < -8.9)
								{
									A2.RelativePoint.Y = -16;
									Touch = Touch_e.GROUND;
								}
								else
								{
									A2.RelativePoint.X = -16;
								}
							}
						}
					}
					break;

				case 10: // 右
					A2.RelativePoint.X = -16;
					break;

				case 3: // 下
					A2.RelativePoint.Y = -16;
					Touch = Touch_e.GROUND;
					break;

				case 6: // 左上と右下
					if (A2.RelativePoint.X < A2.RelativePoint.Y)
					{
						A2.RelativePoint.X = -16;
						A2.RelativePoint.Y = 16;
						Touch = Touch_e.ROOF;
					}
					else
					{
						A2.RelativePoint.X = 16;
						A2.RelativePoint.Y = -16;
						Touch = Touch_e.GROUND;
					}
					break;

				case 11: // 左上のみ空き
					A2.RelativePoint.X = -16;
					A2.RelativePoint.Y = -16;
					Touch = Touch_e.GROUND;
					break;

				default:
					throw null; // never
			}
		}
	}
}
