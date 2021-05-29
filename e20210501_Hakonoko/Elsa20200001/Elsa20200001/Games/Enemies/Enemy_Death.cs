using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Death : Enemy
	{
		public Enemy_Death(D2Point pos)
			: base(pos)
		{ }

		public override void Draw()
		{
			if (!EnemyCommon.IsOutOfScreen_ForDraw(this))
			{
				double p = Math.Sin(DDEngine.ProcFrame / 10.0 + this.X + this.Y) * 0.5 + 0.5; // color phaese

				DDDraw.SetBright(new I3Color(
					SCommon.ToInt(DDUtils.AToBRate(Game.I.Map.Design.EnemyColor_Death_A.R, Game.I.Map.Design.EnemyColor_Death_B.R, p)),
					SCommon.ToInt(DDUtils.AToBRate(Game.I.Map.Design.EnemyColor_Death_A.G, Game.I.Map.Design.EnemyColor_Death_B.G, p)),
					SCommon.ToInt(DDUtils.AToBRate(Game.I.Map.Design.EnemyColor_Death_A.B, Game.I.Map.Design.EnemyColor_Death_B.B, p))
					));
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, SCommon.ToInt(this.X - DDGround.ICamera.X), SCommon.ToInt(this.Y - DDGround.ICamera.Y));
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(GameConsts.TILE_W, GameConsts.TILE_H));

				//Game.I.タイル接近_敵描画_Points.Add(new D2Point(this.X, this.Y)); // 地形の一部なので、追加しない。
			}
		}

		public override Enemy GetClone()
		{
			return new Enemy_Death(new D2Point(this.X, this.Y));
		}
	}
}
