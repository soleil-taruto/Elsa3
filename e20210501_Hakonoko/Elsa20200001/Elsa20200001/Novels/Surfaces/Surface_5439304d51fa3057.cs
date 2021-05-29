using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_吹き出し : Surface
	{
		public static bool Hide = false; // Novel から制御される。

		public Surface_吹き出し(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 60000;
		}

		private double A = 0.0;
		private bool VisibleFlag = false;
		private bool Mirrored = false;
		private double X_Zure = 0.0;
		private double Y_Zure = 0.0;
		private bool Thinking = false;

		private bool IsVisible
		{
			get
			{
				return this.VisibleFlag && !Hide;
			}
		}

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				DDUtils.Approach(ref this.A, this.IsVisible ? 1.0 : 0.0, 0.9);
				DDUtils.Approach(ref this.X_Zure, 0.0, 0.93);
				DDUtils.Approach(ref this.Y_Zure, 0.0, 0.93);

				DDDraw.SetAlpha(this.A);
				DDDraw.DrawBegin(
					this.Thinking ? Ground.I.Picture.Novel_吹き出しThink : Ground.I.Picture.Novel_吹き出し,
					this.X + this.X_Zure,
					this.Y + this.Y_Zure
					);
				DDDraw.DrawZoom_X(1.2);
				DDDraw.DrawZoom_X(this.Mirrored ? -1 : 1);
				DDDraw.DrawEnd();
				DDDraw.Reset();

#if false // 廃止
				// サブタイトル文字列
				{
					int dispSubtitleLength = Math.Min(Novel.I.DispSubtitleCharCount, Novel.I.CurrPage.Subtitle.Length);
					string dispSubtitle = Novel.I.CurrPage.Subtitle.Substring(0, dispSubtitleLength);

					DDFontUtils.DrawString(10, 413, dispSubtitle, DDFontUtils.GetFont("Kゴシック", 16));
				}
#endif

				// シナリオのテキスト文字列
				if (this.IsVisible)
				{
					int dispTextLength = Math.Min(Novel.I.DispCharCount, Novel.I.CurrPage.Text.Length);
					string dispText = Novel.I.CurrPage.Text.Substring(0, dispTextLength);
					string[] dispLines = dispText.Split('\n');

					for (int index = 0; index < dispLines.Length; index++)
					{
						double x = this.X - 240;
						double y = this.Y - 60 + index * 30;

						x += this.X_Zure;
						y += this.Y_Zure;

						DDFontUtils.DrawString((int)x, (int)y, dispLines[index], DDFontUtils.GetFont("Kゴシック", 16), false, new I3Color(110, 100, 90));
					}
				}

				yield return true;
			}
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "Visible")
			{
				this.VisibleFlag = int.Parse(arguments[c++]) != 0;
			}
			else if (command == "Mirror")
			{
				this.Mirrored = int.Parse(arguments[c++]) != 0;
			}
			else if (command == "X-Zure")
			{
				this.X_Zure = double.Parse(arguments[c++]);
			}
			else if (command == "Y-Zure")
			{
				this.Y_Zure = double.Parse(arguments[c++]);
			}
			else if (command == "Think")
			{
				this.Thinking = int.Parse(arguments[c++]) != 0;
			}
			else
			{
				base.Invoke_02(command, arguments);
			}
		}
	}
}
