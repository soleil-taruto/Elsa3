using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	/// <summary>
	/// システム・ロジック的な処理に関わる命令群
	/// </summary>
	public class Surface_System : Surface
	{
		public Surface_System(string typeName, string instanceName)
			: base(typeName, instanceName)
		{ }

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// noop

				yield return true;
			}
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "Swap")
			{
				string name1 = arguments[c++];
				string name2 = arguments[c++];

				foreach (Surface surface in Novel.I.Status.Surfaces)
				{
					if (surface.InstanceName == name1)
						surface.InstanceName = name2;
					else if (surface.InstanceName == name2)
						surface.InstanceName = name1;
				}
			}
			else if (command == "WhileAct")
			{
				while (Novel.I.Status.Surfaces.Any(v => v.Act.Count != 0))
				{
					Novel.I.DrawSurfaces();
					DDEngine.EachFrame();
				}
				DDEngine.FreezeInput(NovelConsts.NEXT_PAGE_INPUT_INTERVAL);
			}
			else if (command == "WhileActOrInput")
			{
				while (Novel.I.Status.Surfaces.Any(v => v.Act.Count != 0))
				{
					// 入力：会話スキップ
					if (DDInput.L.GetInput() == 1)
					{
						Novel.I.会話スキップ_Request = true;
						break;
					}

					// 入力：シナリオを進める。
					if (
						DDMouse.Rot < 0 ||
						DDMouse.L.GetInput() == -1 ||
						DDInput.A.GetInput() == 1
						)
						break;

					Novel.I.DrawSurfaces();
					DDEngine.EachFrame();
				}
				DDEngine.FreezeInput(NovelConsts.NEXT_PAGE_INPUT_INTERVAL);
			}
			//else if (command == "SetCurtain")
			//{
			//    int frameMax = int.Parse(arguments[c++]);
			//    double destWhiteLevel = double.Parse(arguments[c++]);

			//    DDCurtain.SetCurtain(frameMax, destWhiteLevel);
			//}
			else if (command == "NovelEnd")
			{
				Novel.I.会話終了_Request = true;
			}
			else if (command == "A_NovelEnd")
			{
				this.Act.Add(() =>
				{
					Novel.I.会話終了_Request = true;
					return false;
				});
			}
			else
			{
				throw new DDError();
			}
		}
	}
}
