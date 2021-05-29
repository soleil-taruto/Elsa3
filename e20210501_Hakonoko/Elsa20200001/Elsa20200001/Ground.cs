using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

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

		/// <summary>
		/// 0 == クリアしたステージ無し
		/// 1 == テストステージクリア済み -- 本番では取り得ない値
		/// 2 == LAYER 9 クリア済み
		/// 3 == LAYER 8 クリア済み
		/// 4 == LAYER 7 クリア済み
		/// ...
		/// 9 == LAYER 2 クリア済み
		/// 10 == 全ステージクリア済み && 何れかのエンディングに到達済み
		/// </summary>
		public int ReachedStageIndex;

		public bool SawFinalNovel = false;
		public bool SawEnding_死亡 = false;
		public bool SawEnding_生還 = false;
		public bool SawEnding_復讐 = false;

		/// <summary>
		/// 残りリスポーン地点設置回数_初期値
		/// </summary>
		public int StartSnapshotCount = Consts.START_SNAPSHOT_COUNT_DEF;

		public bool 会話スキップ抑止 = false;

		public int CurrStageIndex = 1; // 値域：0～9, 特定のステージに居ない時は値域内の適当な値になっている。
	}
}
