using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public class Ending_生還 : Ending
	{
		protected override IEnumerable<int> Script()
		{
			ClearAllSubScreen();

			Ground.I.Music.Ending_生還.Play();

			DrawWall drawWall = new DrawWall();
			drawWall.DrawPictures.Add(new DrawWall.DrawPicture() { Picture = Ground.I.Picture.WhiteWall });
			DDGround.EL.Add(drawWall.Task);

			// _#Include_Resource // for t20201023_GitHubRepositoriesSolve

			yield return 600;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 200, "\u7729\u3057\u3044\u5149\u304c\u76ee\u306b\u98db\u3073\u8fbc\u3093\u3067\u304f\u308b\u3002", 800)));
			yield return 240;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 250, "\u3069\u3046\u3084\u3089\u79c1\u306f\u751f\u6b7b\u306e\u5883\u304b\u3089\u751f\u9084\u3057\u305f\u3089\u3057\u3044\u3002")));

			drawWall.DrawPictures.Add(new DrawWall.DrawPicture() { Picture = Ground.I.Picture.Ending_生還_背景_01, XAdd = -0.03, YAdd = -0.02 });

			yield return 600;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 250, "\u5468\u308a\u306b\u306f\u5b89\u5835\u306e\u8868\u60c5\u3092\u6d6e\u304b\u3079\u305f\u533b\u5e2b\u3068\u770b\u8b77\u5e2b\u304c\u5c45\u308b\u3002", 1000)));
			yield return 240;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 300, "\u2026\u2026\u305d\u308c\u4ee5\u5916\u306b\u306f\u8ab0\u3082\u5c45\u306a\u3044\u3002", 800)));
			yield return 240;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 350, "\u305d\u308c\u3067\u826f\u3044\u3057\u3001\u3068\u3063\u304f\u306b\u899a\u609f\u306f\u3057\u3066\u3044\u305f\u3002")));

			drawWall.DrawPictures.Add(new DrawWall.DrawPicture() { Picture = Ground.I.Picture.WhiteWall });

			yield return 600;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 150, "\u751f\u304d\u308b\u3053\u3068\u306f\u7d20\u6674\u3089\u3057\u304f\u3001\u305d\u308c\u3060\u3051\u3067\u5e78\u305b\u306a\u3053\u3068\u3002", 800)));
			yield return 240;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 200, "\u305d\u3093\u306a\u6b3a\u779e\u306b\u6e80\u3061\u305f\u8a00\u8449\u306b\u306f\u53cd\u5410\u304c\u51fa\u308b\u3002")));

			drawWall.DrawPictures.Add(new DrawWall.DrawPicture() { Picture = Ground.I.Picture.Ending_生還_背景_02, YAdd = -0.03 });

			yield return 600;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 250, "\u3067\u3082\u3001\u79c1\u306f\u30a2\u30a4\u30c4\u3068\u4e00\u7dd2\u306b\u751f\u304d\u3066\u3044\u304f\u3068\u6c7a\u3081\u305f\u3002", 800)));
			yield return 240;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 300, "\u305d\u308c\u304c\u6b63\u3057\u3044\u9078\u629e\u304b\u3069\u3046\u304b\u306f\u3001\u751f\u304d\u629c\u3044\u3066\u307f\u3066\u521d\u3081\u3066\u5206\u304b\u308b\u3053\u3068\u3002")));

			yield return 600;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 200, "\u7b11\u3063\u3066\u6b7b\u306c\u3002", 1200)));
			yield return 240;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 250, "\u660e\u78ba\u306a\u76ee\u6a19\u304c\u51fa\u6765\u305f\u3053\u3068\u3067\u3001\u5fc3\u306f\u30b9\u30c3\u30ad\u30ea\u3057\u3066\u3044\u308b\u3002", 1000)));
			yield return 440;
			DDGround.EL.Add(SCommon.Supplier(DrawString(480, 330, "\u2026\u2026\u3068\u308a\u3042\u3048\u305a\u3001\u5893\u53c2\u308a\u3068\u9762\u4f1a\u304b\u3089\u59cb\u3081\u3066\u307f\u3088\u3046\u304b\u306a\u3002")));

			yield return 1200;
			DDCurtain.SetCurtain(600, -1.0);
			yield return 300;
			DDMusicUtils.Fade(300);
			yield return 330;

			ClearAllSubScreen();
		}

		private static List<DDSubScreen> SubScreens = new List<DDSubScreen>();

		private static void ClearAllSubScreen()
		{
			while (1 <= SubScreens.Count)
				SCommon.UnaddElement(SubScreens).Dispose();
		}

		private IEnumerable<bool> DrawString(int x, int y, string text, int frameMax = 600)
		{
			DDSubScreen subScreenTmp = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H, true);
			DDSubScreen subScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H, true);
			SubScreens.Add(subScreenTmp);
			SubScreens.Add(subScreen);

			using (subScreenTmp.Section())
			{
				DX.ClearDrawScreen();

				DDFontUtils.DrawString_XCenter(x, y, text, DDFontUtils.GetFont("K\u30b4\u30b7\u30c3\u30af", 30));

				ぼかし効果.Perform(0.01);
			}
			for (int c = 0; c < 3; c++)
			{
				using (subScreen.Section())
				{
					DX.ClearDrawScreen();

					for (int d = 0; d < 30; d++)
					{
						DDDraw.SetBlendAdd(1.0);
						DDDraw.DrawSimple(subScreenTmp.ToPicture(), 0, 0);
						DDDraw.Reset();
					}
					ぼかし効果.Perform(0.01);
				}
				SCommon.Swap(ref subScreen, ref subScreenTmp);
			}
			using (subScreen.Section())
			{
				DX.ClearDrawScreen();

				DDDraw.SetBright(0.0, 0.1, 0.2);
				DDDraw.DrawSimple(subScreenTmp.ToPicture(), 0, 0);
				DDDraw.Reset();

				DDFontUtils.DrawString_XCenter(x, y, text, DDFontUtils.GetFont("K\u30b4\u30b7\u30c3\u30af", 30));
			}

			double a = 0.0;
			double aTarg = 1.0;

			foreach (DDScene scene in DDSceneUtils.Create(frameMax))
			{
				if (scene.Numer == scene.Denom - 300)
					aTarg = 0.0;

				DDUtils.Approach(ref a, aTarg, 0.985);

				DDDraw.SetAlpha(a);
				DDDraw.DrawSimple(subScreen.ToPicture(), 0, 0);
				DDDraw.Reset();

				yield return true;
			}
		}

		private class DrawWall : DDTask
		{
			public List<DrawPicture> DrawPictures = new List<DrawPicture>();

			public override IEnumerable<bool> E_Task()
			{
				for (int frame = 0; ; frame++)
				{
					if (2 <= this.DrawPictures.Count && 1.0 - SCommon.MICRO < this.DrawPictures[1].A)
						this.DrawPictures.RemoveAt(0);

					foreach (DrawPicture task in this.DrawPictures)
						task.Execute();

					yield return true;
				}
			}

			public class DrawPicture : DDTask
			{
				public DDPicture Picture;
				public double XAdd = 0.0;
				public double YAdd = 0.0;

				// <---- prm

				private double X = 0.0;
				private double Y = 0.0;
				public double A = 0.0;

				public override IEnumerable<bool> E_Task()
				{
					for (int frame = 0; ; frame++)
					{
						DDDraw.SetAlpha(this.A);
						DDDraw.DrawSimple(this.Picture, this.X, this.Y);
						DDDraw.Reset();

						this.X += this.XAdd;
						this.Y += this.YAdd;
						DDUtils.Approach(ref this.A, 1.0, 0.993);

						yield return true;
					}
				}
			}
		}
	}
}
