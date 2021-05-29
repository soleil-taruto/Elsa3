using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceSE
	{
		public DDSE Dummy = new DDSE(@"dat\General\muon.wav");

		public DDSE Jump = new DDSE(@"dat\小森平\jump12.mp3");
		public DDSE 拒否 = new DDSE(@"dat\小森平\blip04.mp3");

		public DDSE Goal = new DDSE(@"dat\効果音ラボ\ピアノの単音_wv_[300].mp3");
		public DDSE Miss = new DDSE(@"dat\効果音ラボ\カーソル移動6_wv_[300].mp3");
		public DDSE Death = new DDSE(@"dat\効果音ラボ\メニューを開く2_wv_[300].mp3");
		public DDSE Reborn = new DDSE(@"dat\効果音ラボ\決定、ボタン押下33.mp3"); // wavVolumeでノイズが入るので止む無くそのままで
		public DDSE Snapshot = new DDSE(@"dat\効果音ラボ\カメラのシャッター1_wv_[300].mp3");

		public DDSE EnterPause = new DDSE(@"dat\効果音ラボ\決定、ボタン押下34.mp3");
		public DDSE LeavePause = new DDSE(@"dat\効果音ラボ\決定、ボタン押下35.mp3");
		public DDSE LeavePause_Title = new DDSE(@"dat\効果音ラボ\決定、ボタン押下22_wv_[300].mp3");

		public ResourceSE()
		{
			//this.Dummy.Volume = 0.1; // 非推奨
		}
	}
}
