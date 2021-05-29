using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Novels.Surfaces;

namespace Charlotte.Novels
{
	public class Novel : IDisposable
	{
		public NovelStatus Status = new NovelStatus(); // 軽量な仮設オブジェクト

		// <---- prm

		public bool 会話スキップした = false;

		// <---- ret

		public static Novel I;

		public Novel()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public ScenarioPage CurrPage;
		//public bool SkipMode;
		//public bool AutoMode;

		public int DispSubtitleCharCount;
		public int DispCharCount;
		public int DispPageEndedCount;
		public bool DispFastMode;

		public bool 会話スキップ_Request = false; // ? 会話スキップが入力された(要求された) -> (可能であれば)速やかに会話スキップ(ノベルパート終了)すること。
		public bool 会話終了_Request = false;

		public void Perform()
		{
			DDUtils.SetMouseDispMode(true);

			// reset
			{
				//this.SkipMode = false;
				//this.AutoMode = false;

				Surface_吹き出し.Hide = false;
				//Surface_SystemButtons.Hide = false;
			}

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

		restartCurrPage:
			this.CurrPage = this.Status.Scenario.Pages[this.Status.CurrPageIndex];

			foreach (ScenarioCommand command in this.CurrPage.Commands)
				command.Invoke();

			this.DispSubtitleCharCount = 0;
			this.DispCharCount = 0;
			this.DispPageEndedCount = 0;
			this.DispFastMode = false;

			DDEngine.FreezeInput();

			for (; ; )
			{
				bool nextPageFlag = false;

				// ★★★ キー押下は 1 マウス押下は -1 で判定する。

				// 入力：シナリオを進める。(マウスホイール)
				if (DDMouse.Rot < 0)
				{
					//this.CancelSkipAutoMode(); // 廃止

					if (this.DispPageEndedCount < NovelConsts.NEXT_PAGE_INPUT_INTERVAL) // ? ページ表示_未完了 -> ページ表示_高速化
					{
						this.DispFastMode = true;
					}
					else // ? ページ表示_完了 -> 次ページ
					{
						nextPageFlag = true;
					}
					DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);
				}

				// 入力：シナリオを進める。(マウスホイール_以外)
				if (
					DDMouse.L.GetInput() == -1 ||
					DDInput.A.GetInput() == 1
					)
				{
					if (this.DispPageEndedCount < NovelConsts.NEXT_PAGE_INPUT_INTERVAL) // ? ページ表示_未完了 -> ページ表示_高速化
					{
						this.DispFastMode = true;
					}
					else // ? ページ表示_完了 -> 次ページ
					{
						nextPageFlag = true;
					}
				}

				// 入力：会話スキップ
				if (DDInput.L.GetInput() == 1 || this.会話スキップ_Request)
				{
					this.会話スキップ_Request = false; // reset

					if (Ground.I.会話スキップ抑止)
					{
						Ground.I.SE.拒否.Play();
					}
					else
					{
						this.会話スキップした = true;
						break;
					}
				}
				if (this.会話終了_Request)
					break;

				//if (this.SkipMode)
				//    if (1 <= this.DispPageEndedCount)
				//        nextPageFlag = true;

				//if (this.AutoMode)
				//    if (NovelConsts.AUTO_NEXT_PAGE_INTERVAL <= this.DispPageEndedCount)
				//        nextPageFlag = true;

				if (nextPageFlag) // 次ページ
				{
					// スキップモード時はページを進める毎にエフェクトを強制終了する。
					//if (this.SkipMode)
					//    foreach (Surface surface in this.Status.Surfaces)
					//        surface.Act.Clear();

					this.Status.CurrPageIndex++;

					if (this.Status.Scenario.Pages.Count <= this.Status.CurrPageIndex)
						break;

					goto restartCurrPage;
				}

				// 入力：過去ログ
				if (
					DDInput.DIR_4.GetInput() == 1 ||
					0 < DDMouse.Rot
					)
				{
					this.BackLog();
				}

				// 入力：鑑賞モード
				if (
					DDMouse.R.GetInput() == -1 ||
					DDInput.B.GetInput() == 1
					)
				{
					this.Appreciate();
				}

				if (
					this.CurrPage.Subtitle.Length < this.DispSubtitleCharCount &&
					this.CurrPage.Text.Length < this.DispCharCount
					)
					this.DispPageEndedCount++;

				//if (this.SkipMode)
				//{
				//    this.DispSubtitleCharCount += 8;
				//    this.DispCharCount += 8;
				//}
				//else
				if (this.DispFastMode)
				{
					this.DispSubtitleCharCount += 2;
					this.DispCharCount += 2;
				}
				else
				{
					if (DDEngine.ProcFrame % 2 == 0)
						this.DispSubtitleCharCount++;

					if (DDEngine.ProcFrame % 3 == 0)
						this.DispCharCount++;
				}
				DDUtils.ToRange(ref this.DispSubtitleCharCount, 0, SCommon.IMAX);
				DDUtils.ToRange(ref this.DispCharCount, 0, SCommon.IMAX);

				// ====
				// 描画ここから
				// ====

				this.DrawSurfaces();

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();

				// ★★★ ゲームループの終わり ★★★
			}

			DDCurtain.SetCurtain(10, -1.0);
			DDMusicUtils.Fade();

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				this.DrawSurfaces();

				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();

			DDUtils.SetMouseDispMode(false);

			// ★★★ end of Perform() ★★★
		}

		/// <summary>
		/// スキップモード・オートモードを解除する。
		/// 両モード中、何か入力があれば解除されるのが自然だと思う。
		/// どこで解除しているか分かるようにメソッド化した。
		/// </summary>
		//public void CancelSkipAutoMode()
		//{
		//    this.SkipMode = false;
		//    this.AutoMode = false;
		//}

		/// <summary>
		/// <para>主たる画面描画</para>
		/// <para>色々な場所(モード)から呼び出されるだろう。</para>
		/// </summary>
		public void DrawSurfaces()
		{
			DDCurtain.DrawCurtain(); // 画面クリア

			// Z-オーダー順
			Novel.I.Status.Surfaces.Sort((a, b) =>
			{
				int ret = a.Z - b.Z;
				if (ret != 0)
					return ret;

				ret = SCommon.Comp(a.X, b.X);
				if (ret != 0)
					return ret;

				ret = SCommon.Comp(a.Y, b.Y);
				return ret;
			});

			foreach (Surface surface in Novel.I.Status.Surfaces) // キャラクタ・オブジェクト・壁紙
				if (!surface.Act.Draw())
					surface.Draw();
		}

		/// <summary>
		/// 過去ログ
		/// </summary>
		private void BackLog()
		{
			List<string> logLines = new List<string>();

			for (int index = 0; index < this.Status.CurrPageIndex; index++)
				foreach (string line in this.Status.Scenario.Pages[index].Lines)
					logLines.Add(line);

			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			int backIndex = 0;

			for (; ; )
			{
				if (
					DDMouse.L.GetInput() == -1 ||
					DDInput.A.GetInput() == 1
					)
					break;

				if (
					DDInput.DIR_8.IsPound() ||
					0 < DDMouse.Rot
					)
					backIndex++;

				if (
					DDInput.DIR_2.IsPound() ||
					DDMouse.Rot < 0
					)
					backIndex--;

				if (
					DDInput.DIR_6.GetInput() == 1
					)
					backIndex = -1;

				DDUtils.ToRange(ref backIndex, -1, logLines.Count - 1);

				if (backIndex < 0)
					break;

				this.DrawSurfaces();
				DDCurtain.DrawCurtain(-0.8);

				for (int c = 1; c <= 17; c++)
				{
					int i = logLines.Count - backIndex - c;

					if (0 <= i)
					{
						DDFontUtils.DrawString(8, DDConsts.Screen_H - c * 30 - 8, logLines[i], DDFontUtils.GetFont("Kゴシック", 16));
					}
				}
				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);
		}

		/// <summary>
		/// 鑑賞モード
		/// </summary>
		private void Appreciate()
		{
			Surface_吹き出し.Hide = true;
			//Surface_SystemButtons.Hide = true;

			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			for (; ; )
			{
				// 入力：会話スキップ
				if (DDInput.L.GetInput() == 1)
				{
					this.会話スキップ_Request = true;
					break;
				}

				// 入力：鑑賞モード終了
				if (
					DDMouse.L.GetInput() == -1 ||
					DDMouse.R.GetInput() == -1 ||
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1
					)
					break;

				this.DrawSurfaces();
				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			Surface_吹き出し.Hide = false; // restore
			//Surface_SystemButtons.Hide = false; // restore
		}
	}
}
