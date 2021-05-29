using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameProgressMasters;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			// G,B がほぼ同じなのでバリエーション出ない。

			DDPicture picture = DDCCResource.GetPicture(@"dat\Novel\背景.png");

			DDSubScreen rgb = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen rbg = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen grb = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen gbr = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen brg = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen bgr = new DDSubScreen(picture.Get_W(), picture.Get_H());

			Draw(picture, "RGB", rgb);
			Draw(picture, "RBG", rbg);
			Draw(picture, "GRB", grb);
			Draw(picture, "GBR", gbr);
			Draw(picture, "BRG", brg);
			Draw(picture, "BGR", bgr);

			DDSubScreen[] screens = new DDSubScreen[]
			{
				rgb,
				rbg,
				grb,
				gbr,
				brg,
				bgr,
			};

			int displayIndex = 0;

			for (; ; )
			{
				if (DDInput.DIR_8.IsPound()) displayIndex--;
				if (DDInput.DIR_2.IsPound()) displayIndex++;
				displayIndex += screens.Length;
				displayIndex %= screens.Length;

				DDDraw.DrawSimple(screens[displayIndex].ToPicture(), 0, 0);

				DDEngine.EachFrame();
			}
		}

		public void Test02()
		{
			DDPicture picture = DDCCResource.GetPicture(@"dat\Novel\背景.png");

			DDSubScreen s000 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s001 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s010 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s011 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s100 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s101 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s110 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s111 = new DDSubScreen(picture.Get_W(), picture.Get_H());

			Draw(picture, "RRR", s000);
			Draw(picture, "RRG", s001);
			Draw(picture, "RGR", s010);
			Draw(picture, "RGG", s011);
			Draw(picture, "GRR", s100);
			Draw(picture, "GRG", s101);
			Draw(picture, "GGR", s110);
			Draw(picture, "GGG", s111);

			DDSubScreen[] screens = new DDSubScreen[]
			{
				s000,
				s001,
				s010,
				s011,
				s100,
				s101,
				s110,
				s111,
			};

			int displayIndex = 0;

			for (; ; )
			{
				if (DDInput.DIR_8.IsPound()) displayIndex--;
				if (DDInput.DIR_2.IsPound()) displayIndex++;
				displayIndex += screens.Length;
				displayIndex %= screens.Length;

				DDDraw.DrawSimple(screens[displayIndex].ToPicture(), 0, 0);

				DDEngine.EachFrame();
			}
		}

		public void Test03()
		{
			DDPicture picture = DDCCResource.GetPicture(@"dat\箱から出る\背景.png");

			DDSubScreen s000 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s001 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s010 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s011 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s100 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s101 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s110 = new DDSubScreen(picture.Get_W(), picture.Get_H());
			DDSubScreen s111 = new DDSubScreen(picture.Get_W(), picture.Get_H());

			Draw(picture, "BBB", s000);
			Draw(picture, "BBR", s001);
			Draw(picture, "BRB", s010);
			Draw(picture, "BRR", s011);
			Draw(picture, "RBB", s100);
			Draw(picture, "RBR", s101);
			Draw(picture, "RRB", s110);
			Draw(picture, "RRR", s111);

			DDSubScreen[] screens = new DDSubScreen[]
			{
				s000,
				s001,
				s010,
				s011,
				s100,
				s101,
				s110,
				s111,
			};

			int displayIndex = 0;

			for (; ; )
			{
				if (DDInput.DIR_8.IsPound()) displayIndex--;
				if (DDInput.DIR_2.IsPound()) displayIndex++;
				displayIndex += screens.Length;
				displayIndex %= screens.Length;

				DDDraw.DrawSimple(screens[displayIndex].ToPicture(), 0, 0);

				DDEngine.EachFrame();
			}
		}

		private void Draw(DDPicture src, string colorOrder, DDSubScreen dest)
		{
			Func<char, int> a_charToBlend = chr =>
			{
				switch (chr)
				{
					case 'R': return DX.DX_RGBA_SELECT_BLEND_R;
					case 'G': return DX.DX_RGBA_SELECT_BLEND_G;
					case 'B': return DX.DX_RGBA_SELECT_BLEND_B;

					default:
						throw null; // never
				}
			};

			DX.GraphBlend(
				dest.GetHandle(), // ソース画像かつ出力先
				src.GetHandle(), // ブレンド画像
				255,
				DX.DX_GRAPH_BLEND_RGBA_SELECT_MIX,
				a_charToBlend(colorOrder[0]), // 出力先に適用する R 値
				a_charToBlend(colorOrder[1]), // 出力先に適用する G 値
				a_charToBlend(colorOrder[2]), // 出力先に適用する B 値
				DX.DX_RGBA_SELECT_SRC_A // 出力先に適用する A 値
				);
		}
	}
}
