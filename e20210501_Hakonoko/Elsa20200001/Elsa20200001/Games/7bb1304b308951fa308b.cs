using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.GameProgressMasters;

namespace Charlotte.Games
{
	public static class 箱から出る
	{
		public class Cancelled : Exception
		{ }

		/// <summary>
		/// throws Cancelled
		/// </summary>
		public static void Perform()
		{
			DDUtils.SetMouseDispMode(true);

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain(10);

			DDPicture wallPicture = GameProgressMaster.Get箱から出る背景(Ground.I.CurrStageIndex);

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				DDDraw.DrawSimple(wallPicture, 0, 0);
				DDDraw.DrawCenter(Ground.I.Picture.箱から出る_箱0001, DDConsts.Screen_W / 2, DDConsts.Screen_H - Ground.I.Picture.箱から出る_箱0001.Get_H() / 2);

				P_EachFrame();
			}
			for (int c = 0; c < 2; c++)
			{
				foreach (DDScene scene in DDSceneUtils.Create(30))
				{
					double x = DDConsts.Screen_W / 2;
					double y = DDConsts.Screen_H / 2;

					//double buruSpan = 20.0 * (1.0 - scene.Rate) + 10.0;
					double buruSpan = 30.0 * (1.0 - scene.Rate);
					double xBuru = DDUtils.Random.Real() * buruSpan - buruSpan / 2;
					double yBuru = DDUtils.Random.Real() * buruSpan - buruSpan / 2;
					double z = 1.0 + 0.1 * (1.0 - scene.Rate);

					DDDraw.DrawSimple(wallPicture, 0, 0);
					DDDraw.DrawBegin(Ground.I.Picture.箱から出る_箱0001, x + xBuru, y + yBuru);
					DDDraw.DrawZoom(z);
					DDDraw.DrawEnd();

					P_EachFrame();
				}
			}
			foreach (DDScene scene in DDSceneUtils.Create(80))
			{
				if (scene.Numer == 60)
					DDCurtain.SetCurtain(20, -1.0);

				double x = DDConsts.Screen_W / 2;
				double y = DDConsts.Screen_H / 2 - 10.0;

				double zure;

				if (scene.Numer < 2)
					zure = (1.0 - scene.Numer / 2.0) * 20.0;
				else
					zure = scene.Rate * 20.0;

				double xZure = 0.0;
				double yZure = zure;
				double z = 1.0;

				DDDraw.DrawSimple(wallPicture, 0, 0);
				DDDraw.DrawBegin(Ground.I.Picture.箱から出る_箱0002, x + xZure, y + yZure);
				DDDraw.DrawZoom(z);
				DDDraw.DrawEnd();

				P_EachFrame();
			}
		}

		private static void P_EachFrame()
		{
			// 入力：会話スキップ
			if (DDInput.L.GetInput() == 1)
			{
				if (Ground.I.会話スキップ抑止)
					Ground.I.SE.拒否.Play();
				else
					throw new Cancelled();
			}
			DDEngine.EachFrame();
		}
	}
}
