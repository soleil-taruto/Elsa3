using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_スクリーン : Surface
	{
		private string ImageFile = null; // null == 画像無し
		private double A = 1.0;
		private double SlideRate = 0.5;
		private double DestSlideRate = 0.5;

		public Surface_スクリーン(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 10000;
		}

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.P_Draw();

				yield return true;
			}
		}

		private void P_Draw()
		{
			if (this.ImageFile == null) // ? 画像無し
				return;

			DDUtils.Approach(ref this.SlideRate, this.DestSlideRate, 0.9999);

			DDPicture picture = DDCCResource.GetPicture(this.ImageFile);
			D2Size size = DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H)).Size;

			DDDraw.SetAlpha(this.A);
			DDDraw.DrawRect(
				picture,
				(DDConsts.Screen_W - size.W) * this.SlideRate,
				(DDConsts.Screen_H - size.H) * this.SlideRate,
				size.W,
				size.H
				);
			DDDraw.Reset();
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "レイヤ毎の背景")
			{
				this.Act.AddOnce(() => this.ImageFile = @"dat\背景\Novel_背景_Floor" + Ground.I.CurrStageIndex + ".png");
			}
			else if (command == "画像")
			{
				this.Act.AddOnce(() => this.ImageFile = arguments[c++]);
			}
			else if (command == "A")
			{
				this.Act.AddOnce(() => this.A = double.Parse(arguments[c++]));
			}
			else if (command == "スライド")
			{
				this.Act.AddOnce(() =>
				{
					this.SlideRate = double.Parse(arguments[c++]);
					this.DestSlideRate = double.Parse(arguments[c++]);
				});
			}
			else if (command == "フェードイン")
			{
				this.Act.Add(SCommon.Supplier(this.フェードイン()));
			}
			else if (command == "フェードアウト")
			{
				this.Act.Add(SCommon.Supplier(this.フェードアウト()));
			}
			else
			{
				throw new DDError();
			}
		}

		private IEnumerable<bool> フェードイン()
		{
			foreach (DDScene scene in DDSceneUtils.Create(60))
			{
				if (NovelAct.IsFlush)
				{
					this.A = 1.0;
					break;
				}
				this.A = scene.Rate;
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> フェードアウト()
		{
			foreach (DDScene scene in DDSceneUtils.Create(60))
			{
				if (NovelAct.IsFlush)
				{
					this.A = 0.0;
					break;
				}
				this.A = 1.0 - scene.Rate;
				this.P_Draw();

				yield return true;
			}
		}
	}
}
