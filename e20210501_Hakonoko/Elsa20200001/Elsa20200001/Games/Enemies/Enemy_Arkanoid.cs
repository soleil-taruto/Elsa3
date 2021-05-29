using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Designs;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 敵
	/// アルカノイド
	/// </summary>
	public class Enemy_Arkanoid : Enemy
	{
		private int XAddSign;
		private int YAddSign;
		private double Speed;

		public Enemy_Arkanoid(D2Point pos, int initDirection)
			: base(pos)
		{
			{
				int xa;
				int ya;

				switch (initDirection)
				{
					case 4: xa = -1; ya = 0; break;
					case 6: xa = 1; ya = 0; break;
					case 8: xa = 0; ya = -1; break;
					case 2: xa = 0; ya = 1; break;

					case 1: xa = -1; ya = 1; break;
					case 7: xa = -1; ya = -1; break;
					case 3: xa = 1; ya = 1; break;
					case 9: xa = 1; ya = -1; break;

					default:
						throw null; // never
				}
				this.XAddSign = xa;
				this.YAddSign = ya;
			}

			{
				const double SPEED = 3.19921875; // オリジナルの 3.2 に近い有限桁の値 == 11.00110011(2)
				const double NANAME_SPEED = 2.5;

				if (this.XAddSign * this.YAddSign == 0)
					this.Speed = SPEED;
				else
					this.Speed = NANAME_SPEED;
			}
		}

		public override void Draw()
		{
			if (Game.I.FreezeEnemy)
				goto startDraw;

			this.X += this.Speed * this.XAddSign;
			this.Y += this.Speed * this.YAddSign;

			// 跳ね返り
			{
				const double R = 15.5;
				bool bounced = false;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y)).IsEnemyWall())
				{
					this.XAddSign = 1;
					bounced = true;
				}
				else if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y)).IsEnemyWall())
				{
					this.XAddSign = -1;
					bounced = true;
				}

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y - R)).IsEnemyWall())
				{
					this.YAddSign = 1;
					bounced = true;
				}
				else if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y + R)).IsEnemyWall())
				{
					this.YAddSign = -1;
					bounced = true;
				}

				if (!bounced && this.XAddSign * this.YAddSign != 0) // ? まだ跳ね返っていない && 斜め -> 角に衝突した場合を処理
				{
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y - R)).IsEnemyWall()) // ? 左上に衝突
					{
						this.XAddSign = 1;
						this.YAddSign = 1;
						bounced = true;
					}
					else if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y - R)).IsEnemyWall()) // ? 右上に衝突
					{
						this.XAddSign = -1;
						this.YAddSign = 1;
						bounced = true;
					}
					else if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y + R)).IsEnemyWall()) // ? 左下に衝突
					{
						this.XAddSign = 1;
						this.YAddSign = -1;
						bounced = true;
					}
					else if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y + R)).IsEnemyWall()) // ? 右下に衝突
					{
						this.XAddSign = -1;
						this.YAddSign = -1;
						bounced = true;
					}
				}

				if (bounced)
				{
					double mid_x = (int)(this.X / GameConsts.TILE_W) * GameConsts.TILE_W + GameConsts.TILE_W / 2;
					double mid_y = (int)(this.Y / GameConsts.TILE_H) * GameConsts.TILE_H + GameConsts.TILE_H / 2;

					double dif_x = Math.Abs(this.X - mid_x);
					double dif_y = Math.Abs(this.Y - mid_y);

					this.X = mid_x + dif_x * this.XAddSign;
					this.Y = mid_y + dif_y * this.YAddSign;
				}
			}

		startDraw:
			if (!EnemyCommon.IsOutOfScreen_ForDraw(this))
			{
				//DDDraw.SetBright(new I3Color(150, 100, 200)); // old
				DDDraw.SetBright(Game.I.Map.Design.EnemyColor_Arkanoid);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, SCommon.ToInt(this.X - DDGround.ICamera.X), SCommon.ToInt(this.Y - DDGround.ICamera.Y));
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(GameConsts.TILE_W, GameConsts.TILE_H));

				Game.I.タイル接近_敵描画_Points.Add(new D2Point(this.X, this.Y));
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Arkanoid(new D2Point(this.X, this.Y), 1)
			{
				XAddSign = this.XAddSign,
				YAddSign = this.YAddSign,
				Speed = this.Speed,
			};
		}
	}
}
