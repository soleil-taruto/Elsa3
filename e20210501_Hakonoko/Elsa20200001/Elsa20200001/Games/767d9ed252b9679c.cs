using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class 白黒効果
	{
		private static DDSubScreen GrayScreen_R = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
		private static DDSubScreen GrayScreen_G = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
		private static DDSubScreen GrayScreen_B = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		/// <summary>
		/// 指定スクリーンの内容を白黒(グレースケール)にして画面に描画する。
		/// 指定スクリーンは、メインスクリーンと同じサイズ
		/// </summary>
		/// <param name="screen">指定スクリーン</param>
		public static void Perform(DDSubScreen sourceScreen)
		{
			DX.GraphBlend(
				GrayScreen_R.GetHandle(), // ソース画像かつ出力先
				sourceScreen.GetHandle(), // ブレンド画像
				255,
				DX.DX_GRAPH_BLEND_RGBA_SELECT_MIX,
				DX.DX_RGBA_SELECT_BLEND_R, // 出力先に適用する R 値
				DX.DX_RGBA_SELECT_BLEND_R, // 出力先に適用する G 値
				DX.DX_RGBA_SELECT_BLEND_R, // 出力先に適用する B 値
				DX.DX_RGBA_SELECT_SRC_A // 出力先に適用する A 値
				);

			// DX_RGBA_SELECT_SRC_R == ソース画像の R 値
			// DX_RGBA_SELECT_SRC_G == ソース画像の G 値
			// DX_RGBA_SELECT_SRC_B == ソース画像の B 値
			// DX_RGBA_SELECT_SRC_A == ソース画像の A 値
			// DX_RGBA_SELECT_BLEND_R == ブレンド画像の R 値
			// DX_RGBA_SELECT_BLEND_G == ブレンド画像の G 値
			// DX_RGBA_SELECT_BLEND_B == ブレンド画像の B 値
			// DX_RGBA_SELECT_BLEND_A == ブレンド画像の A 値

			DX.GraphBlend(
				GrayScreen_G.GetHandle(),
				sourceScreen.GetHandle(),
				255,
				DX.DX_GRAPH_BLEND_RGBA_SELECT_MIX,
				DX.DX_RGBA_SELECT_BLEND_G,
				DX.DX_RGBA_SELECT_BLEND_G,
				DX.DX_RGBA_SELECT_BLEND_G,
				DX.DX_RGBA_SELECT_SRC_A
				);

			DX.GraphBlend(
				GrayScreen_B.GetHandle(),
				sourceScreen.GetHandle(),
				255,
				DX.DX_GRAPH_BLEND_RGBA_SELECT_MIX,
				DX.DX_RGBA_SELECT_BLEND_B,
				DX.DX_RGBA_SELECT_BLEND_B,
				DX.DX_RGBA_SELECT_BLEND_B,
				DX.DX_RGBA_SELECT_SRC_A
				);

			//using (XXX.Section()) // 描画先
			{
				DDDraw.DrawSimple(GrayScreen_R.ToPicture(), 0, 0);
				DDDraw.SetAlpha(0.5);
				DDDraw.DrawSimple(GrayScreen_G.ToPicture(), 0, 0);
				DDDraw.SetAlpha(0.333);
				DDDraw.DrawSimple(GrayScreen_B.ToPicture(), 0, 0);
				DDDraw.Reset();
			}
		}
	}
}
