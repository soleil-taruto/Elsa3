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

		public DDMusic Title = new DDMusic(@"dat\夜野ムクロジ\komore-bi_muon-100-100.mp3");
		public DDMusic Novel = new DDMusic(@"dat\夜野ムクロジ\si-piano_muon-100-100.mp3");
		public DDMusic Ending_死亡 = new DDMusic(@"dat\夜野ムクロジ\sito-sito_muon-100-100.mp3");
		public DDMusic Ending_生還 = new DDMusic(@"dat\甘茶の音楽工房\seijakunohoshizora.mp3");
		public DDMusic Ending_復讐 = new DDMusic(@"dat\ユーフルカ\Horror-ginen_loop.ogg").SetLoopByStLength(475726, 3723406);

		public DDMusic Floor_01 = new DDMusic(@"dat\甘茶の音楽工房\orb1_muon-100-100.mp3").SetLoopByStEnd(44100 * 36 + 12345 * 1, 44100 * 84 + 12345 * 1);
		public DDMusic Floor_02 = new DDMusic(@"dat\甘茶の音楽工房\orb2_muon-100-100.mp3").SetLoopByStEnd(44100 * 10 + 1234 * 4, 44100 * 143 + 1234 * 0);
		public DDMusic Floor_03 = new DDMusic(@"dat\甘茶の音楽工房\daremoinaisabaku_muon-100-100.mp3");
		public DDMusic Floor_04 = new DDMusic(@"dat\甘茶の音楽工房\natsunokiri_muon-100-100.mp3");
		public DDMusic Floor_05 = new DDMusic(@"dat\甘茶の音楽工房\amenoprelude_muon-100-100.mp3");
		public DDMusic Floor_06 = new DDMusic(@"dat\甘茶の音楽工房\kanashiminotexture1_muon-100-100.mp3").SetLoopByStEnd(44100 * 1 + 12345 * 5, 44100 * 97 + 12345 * 5);
		public DDMusic Floor_07 = new DDMusic(@"dat\甘茶の音楽工房\moeochirusakura_muon-100-100.mp3").SetLoopByStEnd(793000, 3930000);
		public DDMusic Floor_08 = new DDMusic(@"dat\甘茶の音楽工房\kanashiminotexture2_muon-100-100.mp3").SetLoopByStEnd(654000, 4560000);
		public DDMusic Floor_09 = new DDMusic(@"dat\ユーフルカ\Horror-naraku_loop.ogg").SetLoopByStLength(974485, 1940413);
		public DDMusic FinalZone = new DDMusic(@"dat\ユーフルカ\Horror-NeverLookBack_loop.ogg").SetLoopByStLength(456198, 3097689);

		public DDMusic 地鳴り = new DDMusic(@"dat\DovaSyndrome\ゴゴゴ_激しい地鳴り音.mp3").SetLoopByStEnd(44100 * 1 + 12345 * 1, 44100 * 18 + 12345 * 1);

		public ResourceMusic()
		{
			//this.Dummy.Volume = 0.1; // 非推奨
		}
	}
}
