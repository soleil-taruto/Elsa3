using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public abstract class Ending
	{
		public void Perform()
		{
			DDEngine.FreezeInput();

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			Func<bool> a_script = DDUtils.Scripter(this.Script());

			for (int scrfrm = 0; a_script(); scrfrm++)
			{
				if (DDInput.L.GetInput() == 1) // エンディング_スキップ
				{
					if (Ground.I.会話スキップ抑止)
					{
						Ground.I.SE.拒否.Play();
					}
					else if (180 < scrfrm) // 数秒経過するまでスキップさせない。
					{
						DDEngine.EachFrame();

						DDCurtain.SetCurtain(30, -1.0);
						DDMusicUtils.Fade();

						for (int c = 0; c < 40; c++)
						{
							a_script();

							DDEngine.EachFrame();
						}
						break;
					}
				}
				DDEngine.EachFrame();
			}

			// a_script において以下は実行されているものとする。
			// -- DDCurtain.SetCurtain(30, -1.0);
			// -- DDMusicUtils.Fade();

			foreach (DDScene scene in DDSceneUtils.Create(60)) // 暗転_時間調整
				DDEngine.EachFrame();

			DDGround.EL.Clear();

			DDEngine.FreezeInput();
		}

		protected abstract IEnumerable<int> Script();
	}
}
