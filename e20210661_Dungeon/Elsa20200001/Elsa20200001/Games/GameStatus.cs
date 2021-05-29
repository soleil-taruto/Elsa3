using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	/// <summary>
	/// ゲームの状態を保持する。
	/// プレイヤーのレベルとか保有アイテムといった概念が入ってくることを想定して、独立したクラスとする。
	/// </summary>
	public class GameStatus
	{
		// (テスト時など)特にフィールドを設定せずにインスタンスを生成する使い方を想定して、
		// 全てのパラメータはデフォルト値で初期化すること。

		public Player[] Players = new Player[]
		{
			new Player(),
		};

		// <---- prm
	}
}
