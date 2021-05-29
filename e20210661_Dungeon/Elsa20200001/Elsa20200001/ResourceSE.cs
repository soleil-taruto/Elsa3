using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceSE
	{
		//public DDSE Dummy = new DDSE("Dummy.mp3");

		public DDSE[] テスト用s = new DDSE[]
		{
			new DDSE(@"e20200003_dat\小森平\sfxrse\coin01.mp3"),
			new DDSE(@"e20200003_dat\小森平\sfxrse\coin02.mp3"),
			new DDSE(@"e20200003_dat\小森平\sfxrse\coin04.mp3"),
		};

		public DDSE CrashToWall = new DDSE(@"e20200003_dat\小森平\sfxrse\poka02.mp3");

		public ResourceSE()
		{
			//this.Dummy.Volume = 0.1;
		}
	}
}
