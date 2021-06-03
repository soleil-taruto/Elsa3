using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games;
using Charlotte.Tests;
using Charlotte.Tests.Games;
using Charlotte.Tests.Novels;

namespace Charlotte
{
	public class Program2
	{
		public void Main2()
		{
			try
			{
				Main3();
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main3()
		{
			DDMain2.Perform(Main4);
		}

		private void Main4()
		{
			// *.INIT
			{
				// アプリ固有 >

				波紋効果.INIT();

				// < アプリ固有
			}

			//DDTouch.Touch(); // moved -> Logo

			if (DDConfig.LOG_ENABLED)
			{
				DDEngine.DispDebug = () =>
				{
					DDPrint.SetPrint();
					DDPrint.SetBorder(new I3Color(0, 0, 0));

					DDPrint.DebugPrint(string.Join(
						" ",
						波紋効果.Count,
						Game.I == null ? "-" : "" + Game.I.SnapshotCount,
						Game.I == null ? "-" : "" + Game.I.タイル接近_敵描画_Points.Count,
						"会ス" + (Ground.I.会話スキップ抑止 ? "抑" : "可"),

						Game.I == null ? "-" : "" + Game.I.行き先案内_Crashed_Start方面,
						Game.I == null ? "-" : "" + Game.I.行き先案内_Crashed_Goal方面,

						Game.I == null ? "-" : "" + Game.I.Player.X.ToString("F1"),
						Game.I == null ? "-" : "" + Game.I.Player.Y.ToString("F1"),

						// デバッグ表示する情報をここへ追加..

						DDEngine.FrameProcessingMillis,
						DDEngine.FrameProcessingMillis_Worst
						));

					DDPrint.Reset();
				};
			}

			if (ProcMain.DEBUG)
			{
				Main4_Debug();
			}
			else
			{
				Main4_Release();
			}
		}

		private void Main4_Debug()
		{
			// ---- choose one ----

			//Main4_Release();
			//new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();
			//new Test0002().Test01();
			//new TitleMenuTest().Test01();
			new GameMasterTest().Test01(); // 開始ステージを選択
			//new NovelTest().Test01();
			//new NovelTest().Test02();
			//new NovelTest().Test03(); // 開始シナリオを選択
			//new 箱から出るTest().Test01();
			//new EndingTest().Test_死亡(); // エンディング_死亡
			//new EndingTest().Test_生還(); // エンディング_生還
			//new EndingTest().Test_復讐(); // エンディング_復讐

			// ----
		}

		private void Main4_Release()
		{
			using (new Logo())
			{
				Logo.I.Perform();
			}
			using (new TitleMenu())
			{
				TitleMenu.I.Perform();
			}
		}
	}
}
