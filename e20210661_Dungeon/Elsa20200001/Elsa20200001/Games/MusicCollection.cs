using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class MusicCollection
	{
		public static DDMusic Get(string name)
		{
			if (name == Consts.NAME_DEFAULT)
				return DDGround.GeneralResource.無音;

			DDMusic music;

			switch (name)
			{
				case "Field_01": music = Ground.I.Music.Field_01; break;
				//case "Field_02": music = Ground.I.Music.Field_02; break;
				//case "Field_03": music = Ground.I.Music.Field_03; break;

				// 新しい曲をここへ追加..

				default:
					throw new DDError("name: " + name);
			}
			return music;
		}
	}
}
