using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			for (int frame = 0; ; frame++)
			{
				DDCurtain.DrawCurtain();

				DDPrint.SetPrint(0, 16);
				DDPrint.Print("" + frame);

				switch (frame)
				{
					case 60:
						ProcMain.WriteLog("*1 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // False, False

						DDSoundUtils.Play(Ground.I.Music.Title.Sound.GetHandle(0), false, false);
						//Ground.I.Music.Title.Play(); // タイムラグ有り

						ProcMain.WriteLog("*2 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, True
						break;

					case 120:
						ProcMain.WriteLog("*3 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, True

						DDSoundUtils.Stop(Ground.I.Music.Title.Sound.GetHandle(0));
						//DDMusicUtils.Stop(); // タイムラグ有り

						ProcMain.WriteLog("*4 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, False
						break;

					case 180:
						ProcMain.WriteLog("*5 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, False
						DDSoundUtils.Play(Ground.I.Music.Title.Sound.GetHandle(0), false, false);
						ProcMain.WriteLog("*6 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, True
						break;

					case 240:
						ProcMain.WriteLog("*7 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, True
						DDSoundUtils.Stop(Ground.I.Music.Title.Sound.GetHandle(0));
						ProcMain.WriteLog("*8 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, False
						break;

					case 300:
						ProcMain.WriteLog("*9 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, False
						DDSoundUtils.Play(Ground.I.Music.Title.Sound.GetHandle(0), false, false);
						ProcMain.WriteLog("*10 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, True
						DDSoundUtils.Stop(Ground.I.Music.Title.Sound.GetHandle(0));
						ProcMain.WriteLog("*11 " + Ground.I.Music.Title.Sound.IsLoaded() + ", " + Ground.I.Music.Title.Sound.IsPlaying()); // True, False
						break;

					case 360:
						ProcMain.WriteLog("*12 " + Ground.I.SE.Jump.Sound.IsLoaded() + ", " + Ground.I.SE.Jump.Sound.IsPlaying()); // False, False
						DDSoundUtils.Play(Ground.I.SE.Jump.Sound.GetHandle(0), true, false);
						ProcMain.WriteLog("*13 " + Ground.I.SE.Jump.Sound.IsLoaded() + ", " + Ground.I.SE.Jump.Sound.IsPlaying()); // True, True
						break;
				}
				if (SCommon.IsRange(frame, 361, 419))
				{
					ProcMain.WriteLog("*14 " + Ground.I.SE.Jump.Sound.IsLoaded() + ", " + Ground.I.SE.Jump.Sound.IsPlaying()); // True, True ～ True, False
				}
				DDEngine.EachFrame();
			}
		}
	}
}
