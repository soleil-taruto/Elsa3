using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Meteor : Enemy
	{
		private double Angle;
		private double Speed;
		private I3Color Color;
		private double Rot;
		private double RotAdd;
		private double RotAddAdd;

		public Enemy_Meteor(D2Point pos)
			: base(pos)
		{
			this.Angle = DDUtils.GetAngle(Game.I.Player.X - pos.X, Game.I.Player.Y - pos.Y);
			this.Speed = 0.0;

			{
				int clr = DDUtils.Random.GetInt(192);
				this.Color = new I3Color(255, clr, clr);
			}

			this.Rot = 0.0;
			this.RotAdd = 0.0;
			this.RotAddAdd = DDUtils.Random.DReal() * 0.001;

			DDGround.EL.Add(SCommon.Supplier(this.E_出現エフェクト()));
		}

		private IEnumerable<bool> E_出現エフェクト()
		{
			int frameMax;

			if (Game.I.Player.DeadFrame != 0) // ? プレイヤー死亡
				frameMax = 40;
			else
				frameMax = 20;

			double x = this.X;
			double y = this.Y;
			double zoom;

			if (Game.I.Player.DeadFrame != 0) // ? プレイヤー死亡
				zoom = 4.0;
			else
				zoom = 2.0;

			foreach (DDScene scene in DDSceneUtils.Create(frameMax))
			{
				DDDraw.SetAlpha((1.0 - scene.Rate) * 0.2);
				DDDraw.SetBright(new I3Color(255, 0, 0));

				for (int c = 0; c < 5; c++)
				{
					DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
					DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
					DDDraw.DrawZoom_X(1.0 + DDUtils.Random.Real() * zoom);
					DDDraw.DrawZoom_Y(1.0 + DDUtils.Random.Real() * zoom);
					DDDraw.DrawSlide(
						DDUtils.Random.DReal() * 10.0 * zoom,
						DDUtils.Random.DReal() * 10.0 * zoom
						);
					DDDraw.DrawEnd();
				}
				DDDraw.Reset();

				yield return true;
			}
		}

		public override void Draw()
		{
			if (Game.I.Player.DeadFrame != 0) // ? プレイヤー死亡
				DDUtils.Approach(ref this.Speed, 4.0, 0.9);
			else
				DDUtils.Approach(ref this.Speed, 9.0, 0.975);

			D2Point currSpeed = DDUtils.AngleToPoint(this.Angle, this.Speed);

			this.X += currSpeed.X;
			this.Y += currSpeed.Y;

			DDDraw.SetBright(this.Color);
			DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, SCommon.ToInt(this.X - DDGround.ICamera.X), SCommon.ToInt(this.Y - DDGround.ICamera.Y));
			DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
			DDDraw.DrawRotate(this.Rot);
			DDDraw.DrawEnd();
			DDDraw.Reset();

			this.RotAdd += this.RotAddAdd;
			this.Rot += this.RotAdd;

			//this.Crash = DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(GameConsts.TILE_W, GameConsts.TILE_H)); // old
			this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), GameConsts.TILE_W / 2.0);

			Game.I.タイル接近_敵描画_Points.Add(new D2Point(this.X, this.Y));

			if (
				Game.I.Player.DeadFrame == 0 && // ? プレイヤー生存
				DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)
				)
				this.DeadFlag = true;
		}

		public override Enemy GetClone()
		{
			return new Enemy_Meteor(new D2Point(this.X, this.Y));
		}
	}
}
