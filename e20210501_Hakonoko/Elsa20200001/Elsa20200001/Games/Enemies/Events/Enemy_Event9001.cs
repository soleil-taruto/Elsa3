using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameProgressMasters;
using Charlotte.Novels;

namespace Charlotte.Games.Enemies.Events
{
	public class Enemy_Event9001 : Enemy
	{
		public Enemy_Event9001(D2Point pos)
			: base(pos)
		{ }

		public override void Draw()
		{
			if (DDUtils.GetDistance(new D2Point(this.X, this.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) < 50.0)
			{
				if (Game.I.FinalZone == null) // ? 最終ゾーン未侵入
				{
					DDMusicUtils.Fade();

					this.最終ノベルパート();
					Game.I.FinalZone = new Game.FinalZoneInfo();
					Game.I.Enemies.Add(new Enemy_MeteorLoader(new D2Point(0, 0))); // メテオローダー設置
					Game.I.TakeSnapshot();

					Ground.I.Music.FinalZone.Play();
				}
			}
		}

		private void 最終ノベルパート()
		{
			using (DDSubScreen tmpScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H))
			{
				DDMain.KeepMainScreen();

				using (tmpScreen.Section())
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
				}

				DDCurtain.SetCurtain(0, 0.5);
				DDCurtain.SetCurtain(20);

				foreach (DDScene scene in DDSceneUtils.Create(80))
				{
					if (scene.Numer == scene.Denom - 20)
						DDCurtain.SetCurtain(20, -1.0);

					白黒効果.Perform(tmpScreen);

					DDEngine.EachFrame();
				}

				Ground.I.会話スキップ抑止 = !Ground.I.SawFinalNovel;

				using (new Novel())
				{
					Novel.I.Status.Scenario = GameProgressMaster.I.GetFinalScenario();
					Novel.I.Perform();
				}

				Ground.I.会話スキップ抑止 = false; // restore
				Ground.I.SawFinalNovel = true;

				DDCurtain.SetCurtain(0, -1.0);
				DDCurtain.SetCurtain();

				foreach (DDScene scene in DDSceneUtils.Create(40))
				{
					白黒効果.Perform(tmpScreen);

					DDEngine.EachFrame();
				}

				DDCurtain.SetCurtain(0, 0.5);
				DDCurtain.SetCurtain(20);
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Event9001(new D2Point(this.X, this.Y));
		}
	}
}
