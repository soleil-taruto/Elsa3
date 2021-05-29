using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Cookie : Enemy
	{
		public const int IROT_SPEED = 30;
		public const int IROT_360 = 4000; // 360°

		// RotSpeed, Rot については IROT_360 == 360° となる角度を使用する。

		private int IRotSpeed;
		private int IRot;

		public Enemy_Cookie(D2Point pos, int iRotSpeed, int initIRot)
			: base(pos)
		{
			this.IRotSpeed = iRotSpeed;
			this.IRot = initIRot;
		}

		private const double R = 152.0;

		public override void Draw()
		{
			if (Game.I.FreezeEnemy)
				goto startDraw;

			this.IRot += this.IRotSpeed;

			this.IRot += IROT_360;
			this.IRot %= IROT_360;

		startDraw:
			double rot = this.IRot * (Math.PI * 2.0) / IROT_360;

			double x = this.X + Math.Cos(rot) * R;
			double y = this.Y + Math.Sin(rot) * R;

			if (!EnemyCommon.IsOutOfScreen_ForDraw(new D2Point(x - DDGround.Camera.X, y - DDGround.Camera.Y)))
			{
				//DDDraw.SetBright(new I3Color(32, 192, 32)); // old
				DDDraw.SetBright(Game.I.Map.Design.EnemyColor_Cookie);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, SCommon.ToInt(x - DDGround.ICamera.X), SCommon.ToInt(y - DDGround.ICamera.Y));
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Rect_CenterSize(new D2Point(x, y), new D2Size(GameConsts.TILE_W, GameConsts.TILE_H));

				Game.I.タイル接近_敵描画_Points.Add(new D2Point(x, y));
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Cookie(new D2Point(this.X, this.Y), this.IRotSpeed, this.IRot);
		}
	}
}
