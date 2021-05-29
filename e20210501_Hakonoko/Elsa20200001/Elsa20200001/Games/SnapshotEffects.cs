using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class SnapshotEffects
	{
		private static DDSubScreen EL_Screen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		public static void Perform()
		{
			DDMain.KeepMainScreen();

			DDCurtain.SetCurtain(0, 0.5);
			DDCurtain.SetCurtain(20);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				白黒効果.Perform(DDGround.KeptMainScreen);

				DDEngine.EachFrame();
			}

			// Swap
			{
				DDSubScreen tmp = DDGround.KeptMainScreen;
				DDGround.KeptMainScreen = EL_Screen;
				EL_Screen = tmp;
			}

			DDGround.EL.Add(SCommon.Supplier(E_残像()));
		}

		private static IEnumerable<bool> E_残像()
		{
			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				DDDraw.SetAlpha((1.0 - scene.Rate) * 0.8);
				//DDDraw.DrawSimple(EL_Screen.ToPicture(), 0, 0);
				DDDraw.DrawBegin(EL_Screen.ToPicture(), DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
				DDDraw.DrawZoom(1.0 + scene.Rate * 0.1);
				DDDraw.DrawRotate(scene.Rate * 0.03);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
