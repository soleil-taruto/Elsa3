using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 敵
	/// </summary>
	public abstract class Enemy
	{
		// Game.I.ReloadEnemies() からロードされた場合、初期位置として「配置されたマップセルの中心座標」が与えられる。

		// this.X, this.Y はマップの座標(マップの左上を0,0とする)
		// -- 描画する際は DDGround.ICamera.X, DDGround.ICamera.Y をそれぞれ減じること。

		// リスポーンによる敵再現のため、E_Draw は実装しない。

		public double X;
		public double Y;

		public Enemy(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public Enemy(D2Point pos)
			: this(pos.X, pos.Y)
		{ }

		/// <summary>
		/// この敵を消滅させるか
		/// 撃破された場合などこの敵を消滅させたい場合 true をセットすること。
		/// これにより「フレームの最後に」敵リストから除去される。
		/// </summary>
		public bool DeadFlag = false;

		/// <summary>
		/// 現在のフレームにおける当たり判定を保持する。
		/// -- Draw によって設定される。
		/// </summary>
		public DDCrash Crash = DDCrashUtils.None();

		// リスポーンによる敵再現のため、E_Draw は実装しない。

		/// <summary>
		/// 描画と動作
		/// リスポーンによる敵再現のため、E_Draw は実装しない。
		/// するべきこと：
		/// -- 行動
		/// -- 描画
		/// -- 必要に応じて Crash 設定
		/// </summary>
		public abstract void Draw();

		/// <summary>
		/// この敵のコピーを生成する。
		/// リスポーン時に、この戻り値と置き換わる。
		/// </summary>
		/// <returns>この敵のコピー</returns>
		public abstract Enemy GetClone();
	}
}
