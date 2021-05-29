using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public class MapCell
	{
		/// <summary>
		/// このマップセルが属するマップ
		/// null == デフォルトのマップセル
		/// </summary>
		public Map Parent;

		/// <summary>
		/// 自分自身の座標(X-軸)
		/// </summary>
		public int Self_X;

		/// <summary>
		/// 自分自身の座標(Y-軸)
		/// </summary>
		public int Self_Y;

		public enum Kind_e
		{
			EMPTY,
			START,
			GOAL,
			WALL,
			WALL_ENEMY_THROUGH,
			DEATH,
			ARKANOID_1,
			ARKANOID_2,
			ARKANOID_3,
			ARKANOID_4,
			ARKANOID_6,
			ARKANOID_7,
			ARKANOID_8,
			ARKANOID_9,
			COOKIE_時計回り_1,
			COOKIE_時計回り_2,
			COOKIE_時計回り_3,
			COOKIE_時計回り_4,
			COOKIE_時計回り_6,
			COOKIE_時計回り_7,
			COOKIE_時計回り_8,
			COOKIE_時計回り_9,
			COOKIE_反時計回り_1,
			COOKIE_反時計回り_2,
			COOKIE_反時計回り_3,
			COOKIE_反時計回り_4,
			COOKIE_反時計回り_6,
			COOKIE_反時計回り_7,
			COOKIE_反時計回り_8,
			COOKIE_反時計回り_9,
			PATA_L,
			PATA_L_SLOW,
			PATA_L_FAST,
			PATA_R,
			PATA_R_SLOW,
			PATA_R_FAST,
			EVENT_9001,
			EVENT_9002_2,
			EVENT_9002_4,
			EVENT_9002_6,
			EVENT_9002_8,
			EVENT_9003,
			EVENT_9003B,
			EVENT_9004,
			EVENT_9005,
			行き先案内_Start方面,
			行き先案内_Goal方面,
		}

		/// <summary>
		/// 書式：
		/// -- 名前
		/// -- グループ/名前
		/// -- 名前:表示名
		/// -- 名前:グループ/表示名
		/// </summary>
		public static string[] Kine_e_Names = new string[]
		{
			"空間",
			"スタート地点",
			"ゴール地点",
			"壁",
			"壁_敵通過",
			"即死する壁",
			"A1:アルカノイド/アルカノイド_初期進行方向=左下",
			"A2:アルカノイド/アルカノイド_初期進行方向=下",
			"A3:アルカノイド/アルカノイド_初期進行方向=右下",
			"A4:アルカノイド/アルカノイド_初期進行方向=左",
			"A6:アルカノイド/アルカノイド_初期進行方向=右",
			"A7:アルカノイド/アルカノイド_初期進行方向=左上",
			"A8:アルカノイド/アルカノイド_初期進行方向=上",
			"A9:アルカノイド/アルカノイド_初期進行方向=右上",
			"C1:クッキー_時計回り/クッキー_時計回り_初期位置=左下",
			"C2:クッキー_時計回り/クッキー_時計回り_初期位置=下",
			"C3:クッキー_時計回り/クッキー_時計回り_初期位置=右下",
			"C4:クッキー_時計回り/クッキー_時計回り_初期位置=左",
			"C6:クッキー_時計回り/クッキー_時計回り_初期位置=右",
			"C7:クッキー_時計回り/クッキー_時計回り_初期位置=左上",
			"C8:クッキー_時計回り/クッキー_時計回り_初期位置=上",
			"C9:クッキー_時計回り/クッキー_時計回り_初期位置=右上",
			"H1:クッキー_反時計回り/クッキー_反時計回り_初期位置=左下",
			"H2:クッキー_反時計回り/クッキー_反時計回り_初期位置=下",
			"H3:クッキー_反時計回り/クッキー_反時計回り_初期位置=右下",
			"H4:クッキー_反時計回り/クッキー_反時計回り_初期位置=左",
			"H6:クッキー_反時計回り/クッキー_反時計回り_初期位置=右",
			"H7:クッキー_反時計回り/クッキー_反時計回り_初期位置=左上",
			"H8:クッキー_反時計回り/クッキー_反時計回り_初期位置=上",
			"H9:クッキー_反時計回り/クッキー_反時計回り_初期位置=右上",
			"PL2:パタパタ/パタパタ_初期進行方向=左",
			"PL1:パタパタ/パタパタ_初期進行方向=左_低速",
			"PL3:パタパタ/パタパタ_初期進行方向=左_高速",
			"PR2:パタパタ/パタパタ_初期進行方向=右",
			"PR1:パタパタ/パタパタ_初期進行方向=右_低速",
			"PR3:パタパタ/パタパタ_初期進行方向=右_高速",
			"EVENT/EVENT_9001",
			"EVENT/EVENT_9002_2",
			"EVENT/EVENT_9002_4",
			"EVENT/EVENT_9002_6",
			"EVENT/EVENT_9002_8",
			"EVENT/EVENT_9003",
			"EVENT/EVENT_9003B",
			"EVENT/EVENT_9004",
			"EVENT/EVENT_9005",
			"先S:EVENT/行き先案内_Start方面",
			"先G:EVENT/行き先案内_Goal方面",
		};

		public static I3Color[] Kind_e_Colors = new I3Color[]
		{
			new I3Color(   0,   0,   0 ), // 空間
			new I3Color( 255, 255, 255 ), // スタート地点
			new I3Color(   0, 162, 232 ), // ゴール地点
			new I3Color( 255, 242,   0 ), // 壁
			new I3Color( 255, 242,   1 ), // 壁_敵通過
			new I3Color( 237,  28,  36 ), // 即死する壁
			new I3Color(  63,  72, 207 ), // 青い箱 1
			new I3Color(  63,  72, 206 ), // 青い箱 2
			new I3Color(  63,  72, 205 ), // 青い箱 3
			new I3Color(  63,  72, 208 ), // 青い箱 4
			new I3Color(  63,  72, 204 ), // 青い箱 6
			new I3Color(  63,  72, 209 ), // 青い箱 7
			new I3Color(  63,  72, 210 ), // 青い箱 8
			new I3Color(  63,  72, 211 ), // 青い箱 9
			new I3Color(  34, 177,  79 ), // 緑の箱 T1
			new I3Color(  34, 177,  78 ), // 緑の箱 T2
			new I3Color(  34, 177,  77 ), // 緑の箱 T3
			new I3Color(  34, 177,  80 ), // 緑の箱 T4
			new I3Color(  34, 177,  76 ), // 緑の箱 T6
			new I3Color(  34, 177,  81 ), // 緑の箱 T7
			new I3Color(  34, 177,  82 ), // 緑の箱 T8
			new I3Color(  34, 177,  83 ), // 緑の箱 T9
			new I3Color(  34, 177,  87 ), // 緑の箱 H1
			new I3Color(  34, 177,  86 ), // 緑の箱 H2
			new I3Color(  34, 177,  85 ), // 緑の箱 H3
			new I3Color(  34, 177,  88 ), // 緑の箱 H4
			new I3Color(  34, 177,  84 ), // 緑の箱 H6
			new I3Color(  34, 177,  89 ), // 緑の箱 H7
			new I3Color(  34, 177,  90 ), // 緑の箱 H8
			new I3Color(  34, 177,  91 ), // 緑の箱 H9
			new I3Color( 255, 174, 201 ), // 赤い箱 L
			new I3Color( 255, 174, 202 ), // 赤い箱 LS
			new I3Color( 255, 174, 203 ), // 赤い箱 LF
			new I3Color( 255, 174, 204 ), // 赤い箱 R
			new I3Color( 255, 174, 205 ), // 赤い箱 RS
			new I3Color( 255, 174, 206 ), // 赤い箱 RF
			new I3Color(   0,   0,   1 ), // EVENT_9001
			new I3Color(   0,   0,   2 ), // EVENT_9002_2
			new I3Color(   0,   0,   3 ), // EVENT_9002_4
			new I3Color(   0,   0,   4 ), // EVENT_9002_6
			new I3Color(   0,   0,   5 ), // EVENT_9002_8
			new I3Color(   0,   0,   6 ), // EVENT_9003
			new I3Color(   0,   1,   6 ), // EVENT_9003B
			new I3Color(   0,   0,   7 ), // EVENT_9004
			new I3Color(   0,   0,   8 ), // EVENT_9005
			new I3Color(   0,   0,   9 ), // 行き先案内_Start方面
			new I3Color(   0,   0,  10 ), // 行き先案内_Goal方面
		};

		public Kind_e Kind;
		public Kind_e? KindOrig = null; // リスポーン時_復元用

		/// <summary>
		/// 色位相
		/// マップロード時にランダムにセットされる。
		/// 0.0 ～ 1.0
		/// </summary>
		public double ColorPhase;

		// <---- prm

		public double ColorPhaseShift = 0.0; // -1.0 ～ 1.0

		/// <summary>
		/// このマップセルは「デフォルトのマップセル」か
		/// </summary>
		public bool IsDefault
		{
			get
			{
				return this == GameCommon.DefaultMapCell;
			}
		}

		/// <summary>
		/// 同じマップ上の別のマップセルを取得する。
		/// </summary>
		/// <param name="x">マップ上の座標(X-軸)</param>
		/// <param name="y">マップ上の座標(Y-軸)</param>
		/// <returns>マップセル</returns>
		public MapCell GetAnotherCell(int x, int y)
		{
			if (this.Parent == null)
				return GameCommon.DefaultMapCell;

			if (
				x < 0 || this.Parent.W <= x ||
				y < 0 || this.Parent.H <= y
				)
				return GameCommon.DefaultMapCell;

			return this.Parent.Table[x, y];
		}

		public bool IsGoal()
		{
			return this.Kind == Kind_e.GOAL;
		}

		public bool IsCookie()
		{
			return SCommon.IsRange((int)this.Kind, (int)Kind_e.COOKIE_時計回り_1, (int)Kind_e.COOKIE_反時計回り_9);
		}

		/// <summary>
		/// プレイヤーにとっての壁かどうか判定する。
		/// </summary>
		/// <returns>プレイヤーにとっての壁か</returns>
		public bool IsWall()
		{
			return
				this.Kind == Kind_e.WALL ||
				this.Kind == Kind_e.WALL_ENEMY_THROUGH ||
				this.IsCookie();
		}

		/// <summary>
		/// 敵にとっての壁かどうか判定する。
		/// </summary>
		/// <returns>敵にとっての壁か</returns>
		public bool IsEnemyWall()
		{
			return
				this.Kind == Kind_e.WALL ||
				this.Kind == Kind_e.GOAL ||
				this.Kind == Kind_e.DEATH ||
				this.IsCookie();
		}

		/// <summary>
		/// 敵にとっての壁かどうか判定する。
		/// 但し、1マス分の通路も壁と見なす。
		/// </summary>
		/// <returns>敵にとっての壁か</returns>
		public bool IsEnemyWall_NoNarrow()
		{
			return
				this.IsEnemyWall() ||
				(
					this.GetAnotherCell(this.Self_X - 1, this.Self_Y).IsEnemyWall() &&
					this.GetAnotherCell(this.Self_X + 1, this.Self_Y).IsEnemyWall()
				)
				||
				(
					this.GetAnotherCell(this.Self_X, this.Self_Y - 1).IsEnemyWall() &&
					this.GetAnotherCell(this.Self_X, this.Self_Y + 1).IsEnemyWall()
				);
		}

		// Design_0001 用 >

		public double 敵接近_Rate = 0.0;

		// < Design_0001 用
	}
}
