using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Designs;

namespace Charlotte.Games.Enemies.Events
{
	public class Enemy_Event9003 : Enemy
	{
		public Enemy_Event9003(D2Point pos)
			: base(pos)
		{ }

		public override void Draw()
		{
			if (DDUtils.GetDistance(new D2Point(this.X, this.Y), new D2Point(Game.I.Player.X, Game.I.Player.Y)) < 50.0)
			{
				if (Game.I.FinalZone.OH_Event9003.Once())
				{
					for (int x = 0; x < Game.I.Map.W; x++)
					{
						for (int y = 0; y < Game.I.Map.H; y++)
						{
							MapCell cell = Game.I.Map.GetCell(x, y);

							if (
								cell.Kind == MapCell.Kind_e.DEATH ||
								cell.Kind == MapCell.Kind_e.GOAL
								)
								cell.Kind = MapCell.Kind_e.WALL;
						}
					}

					// HACK
					// memo 2021.7.3
					// 背景真っ赤ゾーン突入と敵衝突が同じフレームで起きた場合、想定外の状態になりそうだ。
					// -- 敵に当たって this.Player.DeadFrame = 1; してからこのイベントが発動した場合
					// --> 背景真っ赤なまま死亡、リスポーンできず(背景真っ赤ゾーンはリスポーン不可)に死亡エンド突入するっぽい。-- 落ちたりしないが、画面上おかしくなる。
					// 但し Game.I.Enemies においてイベントは敵よりも前にあるはず。(敵は後からどんどん追加されるため)
					// 同じフレームにおいてイベント・敵の接触が同時に発生しても、イベントの方が先に発動(this.Draw)して敵が消去されるので、敵の Draw は実行されず
					// 前述の異常は起きないはず。
					// -- Game.I.Enemies においてイベントは敵より前という前提であることに注意！-- 現時点の実装はそうであるはず。

					//Game.I.Player.DeadFrame = 1; // test

					// 必要なイベント以外を除去
					//
					Game.I.Enemies.RemoveAll(enemy => !(
						enemy is Enemy_Event9003B ||
						enemy is Enemy_Event9004 ||
						enemy is Enemy_Event9005
						));

					Game.I.Map.Design = new Design_0002();

					//DDGround.EL.Add(SCommon.Supplier(Effects.Liteフラッシュ()));
					DDGround.EL.Add(SCommon.Supplier(Effects.Heavyフラッシュ()));

					DDMusicUtils.Stop();
				}
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Event9003(new D2Point(this.X, this.Y));
		}
	}
}
