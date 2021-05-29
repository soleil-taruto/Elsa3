using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_キャラクタ : Surface
	{
		public double Draw_Rnd = DDUtils.Random.Real() * Math.PI * 2.0;

		public static string[] CHARA_NAMES = new string[]
		{
			"箱",
			"少女",
		};

		private class ImageInfo
		{
			public string Name;
			public DDPicture Image;

			public ImageInfo(string name, DDPicture image)
			{
				this.Name = name;
				this.Image = image;
			}
		}

		private ImageInfo[][] ImageTable = new ImageInfo[][]
		{
			new ImageInfo[] // 箱
			{
				new ImageInfo("普", Ground.I.Picture.Novel_箱),
			},
			new ImageInfo[] // 少女
			{
				new ImageInfo("普", Ground.I.Picture.Novel_少女_普),
				new ImageInfo("怒", Ground.I.Picture.Novel_少女_怒),
			},
		};

		public int Chara = 0; // 箱
		public int Mode = 0; // 普
		public double A = 1.0;
		public double Zoom = 1.0;
		public bool Mirrored = false;

		public Surface_キャラクタ(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 20000;
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
			const double BASIC_ZOOM = 1.0;

			DDDraw.SetAlpha(this.A);
			DDDraw.DrawBegin(this.ImageTable[(int)this.Chara][this.Mode].Image, this.X, this.Y + (Math.Sin(DDEngine.ProcFrame / 67.0 + this.Draw_Rnd) + 1.0) * 2.0);
			DDDraw.DrawZoom(BASIC_ZOOM * this.Zoom);
			DDDraw.DrawZoom_X(this.Mirrored ? -1 : 1);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "Chara")
			{
				this.Act.AddOnce(() =>
				{
					string charaName = arguments[c++];
					int chara = SCommon.IndexOf(CHARA_NAMES, charaName);

					if (chara == -1)
						throw new DDError("Bad chara: " + charaName);

					this.Chara = chara;
				});
			}
			else if (command == "Mode")
			{
				this.Act.AddOnce(() =>
				{
					string modeName = arguments[c++];
					int mode = SCommon.IndexOf(this.ImageTable[this.Chara], v => v.Name == modeName);

					if (mode == -1)
						throw new DDError("Bad mode: " + mode);

					this.Mode = mode;
				});
			}
			else if (command == "A")
			{
				this.Act.AddOnce(() => this.A = double.Parse(arguments[c++]));
			}
			else if (command == "Zoom")
			{
				this.Act.AddOnce(() => this.Zoom = double.Parse(arguments[c++]));
			}
			else if (command == "Mirror")
			{
				this.Act.AddOnce(() => this.Mirrored = int.Parse(arguments[c++]) != 0);
			}
			else if (command == "待ち")
			{
				int frame = int.Parse(arguments[c++]);

				this.Act.Add(SCommon.Supplier(this.待ち(frame)));
			}
			else if (command == "フェードイン")
			{
				this.Act.Add(SCommon.Supplier(this.フェードイン()));
			}
			else if (command == "フェードアウト")
			{
				this.Act.Add(SCommon.Supplier(this.フェードアウト()));
			}
			else if (command == "モード変更")
			{
				string modeName = arguments[c++];

				this.Act.Add(SCommon.Supplier(this.モード変更(modeName)));
			}
			else if (command == "モード変更_Mirror")
			{
				string modeName = arguments[c++];
				bool mirrored = int.Parse(arguments[c++]) != 0;

				this.Act.Add(SCommon.Supplier(this.モード変更(modeName, mirrored)));
			}
			else if (command == "スライド")
			{
				double x = double.Parse(arguments[c++]);
				double y = double.Parse(arguments[c++]);

				this.Act.Add(SCommon.Supplier(this.スライド(x, y)));
			}
			else if (command == "Walk")
			{
				double x = double.Parse(arguments[c++]);

				this.Act.Add(SCommon.Supplier(this.Walk(x)));
			}
			else
			{
				ProcMain.WriteLog(command);
				throw new DDError();
			}
		}

		private IEnumerable<bool> 待ち(int frame)
		{
			foreach (DDScene scene in DDSceneUtils.Create(frame))
			{
				if (NovelAct.IsFlush)
					break;

				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> フェードイン()
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
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
			foreach (DDScene scene in DDSceneUtils.Create(10))
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

		private IEnumerable<bool> モード変更(string modeName)
		{
			return this.モード変更(modeName, null); // 最初の yield return を待たずに、この行は評価されることに注意！
		}

		private IEnumerable<bool> モード変更(string modeName, bool? mirrored)
		{
			int mode = SCommon.IndexOf(this.ImageTable[this.Chara], v => v.Name == modeName);

			if (mode == -1)
				throw new DDError("Bad mode: " + mode);

			int currMode = this.Mode;
			int destMode = mode;
			bool currMirrored = this.Mirrored;
			bool destMirrored = mirrored == null ? this.Mirrored : mirrored.Value;

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				if (NovelAct.IsFlush)
				{
					this.A = 1.0;
					this.Mode = destMode;
					this.Mirrored = destMirrored;
					break;
				}
				this.A = DDUtils.Parabola(scene.Rate * 0.5 + 0.5);
				this.Mode = currMode;
				this.Mirrored = currMirrored;
				this.P_Draw();

				this.A = DDUtils.Parabola(scene.Rate * 0.5 + 0.0);
				this.Mode = destMode;
				this.Mirrored = destMirrored;
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> スライド(double x, double y)
		{
			double currX = this.X;
			double destX = x;
			double currY = this.Y;
			double destY = y;

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				if (NovelAct.IsFlush)
				{
					this.X = destX;
					this.Y = destY;
					break;
				}
				this.X = DDUtils.AToBRate(currX, destX, DDUtils.SCurve(scene.Rate));
				this.Y = DDUtils.AToBRate(currY, destY, DDUtils.SCurve(scene.Rate));
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> Walk(double x)
		{
			double currX = this.X;
			double destX = x;
			double currY = this.Y;

			Action a_flush = () =>
			{
				this.X = destX;
				this.Y = currY;
			};

			const int STEP_NUM = 3;
			const double Y_SPAN = 10.0;

			for (int step = 0; step < STEP_NUM; step++)
			{
				foreach (DDScene scene in DDSceneUtils.Create(15))
				{
					if (NovelAct.IsFlush)
					{
						a_flush();
						yield break;
					}
					this.X = DDUtils.AToBRate(currX, destX, ((step * 2 + 0) + scene.Rate) / (STEP_NUM * 2));
					this.Y = currY + Y_SPAN * (1.0 - DDUtils.Parabola(scene.Rate * 0.5 + 0.5));
					this.P_Draw();

					yield return true;
				}
				foreach (DDScene scene in DDSceneUtils.Create(15))
				{
					if (NovelAct.IsFlush)
					{
						a_flush();
						yield break;
					}
					this.X = DDUtils.AToBRate(currX, destX, ((step * 2 + 1) + scene.Rate) / (STEP_NUM * 2));
					this.Y = currY + Y_SPAN * (1.0 - DDUtils.Parabola(scene.Rate * 0.5 + 0.0));
					this.P_Draw();

					yield return true;
				}
			}
		}
	}
}
