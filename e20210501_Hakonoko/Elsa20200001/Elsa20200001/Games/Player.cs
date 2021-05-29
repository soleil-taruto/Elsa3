using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;

namespace Charlotte.Games
{
	/// <summary>
	/// プレイヤーに関する情報と機能
	/// 唯一のインスタンスを Game.I.Player に保持する。
	/// </summary>
	public class Player
	{
		public double X;
		public double Y;
		public double XSpeed;
		public double YSpeed;
		public int MoveFrame;
		public bool MoveSlow; // ? 低速移動
		public int JumpCount;
		public int JumpFrame;
		public int AirborneFrame; // 0 == 接地状態, 1～ == 滞空状態
		public int DeadFrame = 0; // 0 == 無効, 1～ == 死亡中
		public int RebornFrame = 0; // 0 == 無効, 1～ == 登場中

		public void Draw()
		{
			Game.I.Map.Design.DrawPlayer();
		}

		public void Draw_02()
		{
			if (1 <= this.DeadFrame)
			{
				this.DrawOnDead((double)this.DeadFrame / GameConsts.PLAYER_DEAD_FRAME_MAX);
				return;
			}
			if (1 <= this.RebornFrame)
			{
				this.DrawOnDead(1.0 - (double)this.RebornFrame / GameConsts.PLAYER_REBORN_FRAME_MAX);
				return;
			}

			DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, SCommon.ToInt(this.X - DDGround.ICamera.X), SCommon.ToInt(this.Y - DDGround.ICamera.Y));
			DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
			DDDraw.DrawEnd();
		}

		private void DrawOnDead(double rate)
		{
			DDDraw.SetAlpha(0.3);

			for (int c = 0; c < 5; c++)
			{
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);

				double hir = 1.0 + DDUtils.Random.Real() * rate * 3.0;
				double lwr = 1.0 / hir;

				hir *= hir;
				hir *= hir;

				if (DDUtils.Random.Real() < 0.5)
				{
					DDDraw.DrawZoom_X(hir);
					DDDraw.DrawZoom_Y(lwr);
				}
				else
				{
					DDDraw.DrawZoom_X(lwr);
					DDDraw.DrawZoom_Y(hir);
				}

				DDDraw.DrawSlide(
					(DDUtils.Random.Real() - 0.5) * 2.0 * 40.0 * rate,
					(DDUtils.Random.Real() - 0.5) * 2.0 * 40.0 * rate
					);
				DDDraw.DrawEnd();
			}
			DDDraw.Reset();
		}

		public void Attack()
		{
			// noop
		}
	}
}
