using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameProgressMasters;

namespace Charlotte.Games
{
	public static class レイヤ表示
	{
		/// <summary>
		/// レイヤ表示を実行する。
		/// ステージ番号：
		/// -- 1～9 == 各ステージ
		/// </summary>
		/// <param name="stageNo">ステージ番号</param>
		/// <param name="themeColor">このステージの印象的な色</param>
		public static void Perform(int stageNo, I3Color themeColor, DDMusic music)
		{
			int layerNo = 10 - stageNo;

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			using (DDSubScreen tmpScreen = new DDSubScreen(400, 200))
			{
				foreach (DDScene scene in DDSceneUtils.Create(210))
				{
					if (scene.Numer == 30)
					{
						DDTouch.Touch();
						music.Play();
					}
					if (scene.Numer + 30 == scene.Denom)
						DDCurtain.SetCurtain(30, -1.0);

					DDCurtain.DrawCurtain();

					int bure = (int)(scene.Rate * scene.Rate * 20);
					int xBure = DDUtils.Random.GetRange(-bure, bure);
					int yBure = DDUtils.Random.GetRange(-bure, bure);
#if true
					DDFontUtils.DrawString_XCenter(
						DDConsts.Screen_W / 2 + xBure,
						DDConsts.Screen_H / 2 + yBure - 50,
						"LAYER " + layerNo,
						DDFontUtils.GetFont("03焚火-Regular", 100)
						);
#else
					using (tmpScreen.Section())
					{
						DDPrint.SetColor(new I3Color(60, 60, 60));
						DDPrint.SetBorder(new I3Color(255, 255, 255));
						DDPrint.SetPrint(tmpScreen.GetSize().W / 2 - 60, tmpScreen.GetSize().H / 2 - 8);
						DDPrint.Print("L A Y E R : " + layerNo);
						DDPrint.Reset();
					}
					DDDraw.SetMosaic();
					DDDraw.DrawBegin(
						tmpScreen.ToPicture(),
						DDConsts.Screen_W / 2 + xBure,
						DDConsts.Screen_H / 2 + yBure
						);
					DDDraw.DrawZoom(6.0);
					DDDraw.DrawEnd();
					DDDraw.Reset();
#endif

					if (0.5 < scene.Rate)
					{
						const int c_max = 300;

						for (int c = (int)(scene.Rate * scene.Rate * c_max); 0 < c; c--)
						{
							double c_rate = (double)c / c_max;

							DDDraw.SetAlpha(scene.Rate * 0.5);
							//DDDraw.SetBright(new I3Color(c, c, c)); // old
							DDDraw.SetBright(new I3Color(
								(int)(themeColor.R * c_rate),
								(int)(themeColor.G * c_rate),
								(int)(themeColor.B * c_rate)
								));
							DDDraw.DrawBegin(
								Ground.I.Picture.WhiteBox,
								DDUtils.Random.GetInt(DDConsts.Screen_W),
								DDUtils.Random.GetInt(DDConsts.Screen_H)
								);
							DDDraw.DrawSetSize(
								DDUtils.Random.GetRange(40, (int)(400 * scene.Rate)),
								DDUtils.Random.GetRange(20, (int)(200 * scene.Rate))
								);
							DDDraw.DrawEnd();
							DDDraw.Reset();
						}
					}

					DDEngine.EachFrame();
				}
			}

			DDCurtain.SetCurtain(0);
		}
	}
}
