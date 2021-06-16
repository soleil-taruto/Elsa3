using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Novels;

namespace Charlotte
{
	public class Ground
	{
		public static Ground I;

		public ResourceMusic Music = new ResourceMusic();
		public ResourcePicture Picture = new ResourcePicture();
		public ResourcePicture2 Picture2;
		public ResourceSE SE = new ResourceSE();

		// DDSaveData.Save/Load でセーブ・ロードする情報はここに保持する。

		public int NovelMessageSpeed = NovelConsts.MESSAGE_SPEED_DEF;

		public enum スキップ設定_e
		{
			既読のみ = 1,
			未読も含む,
		};

		public スキップ設定_e スキップ設定 = スキップ設定_e.既読のみ;

		public enum ショットのタイミング_e
		{
			ショットボタンを押し下げた時 = 1,
			ショットボタンを離した時,
		};

		public ショットのタイミング_e ショットのタイミング = ショットのタイミング_e.ショットボタンを押し下げた時;
	}
}
