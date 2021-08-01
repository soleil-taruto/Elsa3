using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceMusic
	{
		public DDMusic Dummy = new DDMusic(@"dat\General\muon.wav");

		public DDMusic Title = new DDMusic(@"dat\hmix\n118.mp3");

		public DDMusic Stage_01 = new DDMusic(@"dat\hmix\n138.mp3");
		public DDMusic Stage_02 = new DDMusic(@"dat\hmix\n70.mp3");
		public DDMusic Stage_03 = new DDMusic(@"dat\hmix\n13.mp3");

		// memo: ループ開始・終了位置を探すツール --> C:\Dev\wb\t20201022_SoundLoop

		public DDMusic Boss_01 = new DDMusic(@"dat\ユーフルカ\Battle-Vampire_loop\Battle-Vampire_loop.ogg").SetLoopByStLength(241468, 4205876);
		public DDMusic Boss_02 = new DDMusic(@"dat\ユーフルカ\Battle-Conflict_loop\Battle-Conflict_loop.ogg").SetLoopByStLength(281888, 3704134);
		public DDMusic Boss_03 = new DDMusic(@"dat\ユーフルカ\Battle-rapier_loop\Battle-rapier_loop.ogg").SetLoopByStLength(422312, 2767055);

		public ResourceMusic()
		{
			//this.Dummy.Volume = 0.1; // 非推奨
		}
	}
}
