using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public static class SurfaceCreator
	{
		private class Info
		{
			public string TypeName;
			public Func<Surface> CreateSurface;

			public Info(string typeName, Func<Surface> createSurface)
			{
				this.TypeName = typeName;
				this.CreateSurface = createSurface;
			}
		}

		private static string _tn;
		private static string _in;

		private static Info[] Infos = new Info[]
		{
			new Info("System", () => new Surface_System(_tn, _in)),
			new Info("吹き出し", () => new Surface_吹き出し(_tn, _in)),
			new Info("キャラクタ", () => new Surface_キャラクタ(_tn, _in)),
			new Info("スクリーン", () => new Surface_スクリーン(_tn, _in)),
			new Info("音楽", () => new Surface_音楽(_tn, _in)),
			new Info("効果音", () => new Surface_効果音(_tn, _in)),

			// 新しいサーフェスをここへ追加..
		};

		public static Surface Create(string typeName, string instanceName)
		{
			int index = SCommon.IndexOf(Infos, v => v.TypeName == typeName);

			if (index == -1)
				throw new DDError("不明なタイプ名：" + typeName);

			_tn = typeName;
			_in = instanceName;

			Surface surface = Infos[index].CreateSurface();

			_tn = null;
			_in = null;

			return surface;
		}
	}
}
