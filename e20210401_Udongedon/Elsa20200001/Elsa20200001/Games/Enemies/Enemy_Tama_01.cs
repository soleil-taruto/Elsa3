using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 自機狙い弾・偶数弾・奇数弾を想定した普通の敵弾
	/// </summary>
	public class Enemy_Tama_01 : Enemy
	{
		public EnemyCommon.TAMA_KIND_e TamaKind;
		public EnemyCommon.TAMA_COLOR_e TamaColor;
		public double Speed;
		public double Angle;

		public Enemy_Tama_01(double x, double y, EnemyCommon.TAMA_KIND_e tamaKind, EnemyCommon.TAMA_COLOR_e tamaColor, double speed, double angle, int absorbableWeapon = -1)
			: base(x, y, Kind_e.TAMA, 0, 0, absorbableWeapon)
		{
			// x
			// y
			// tamaKind
			// tamaColor
			if (speed < 0.1 || 100.0 < speed) throw new DDError();
			if (angle < -3.0 * Math.PI || 3.0 * Math.PI < angle) throw new DDError();

			this.TamaKind = tamaKind;
			this.TamaColor = tamaColor;
			this.Speed = speed;
			this.Angle = angle;

			this.SetXYSpeed();
		}

		public double XAdd;
		public double YAdd;

		private void SetXYSpeed()
		{
			DDUtils.MakeXYSpeed(this.X, this.Y, Game.I.Player.X, Game.I.Player.Y, this.Speed, out this.XAdd, out this.YAdd);
			DDUtils.Rotate(ref this.XAdd, ref this.YAdd, this.Angle);
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double r = EnemyCommon_Tama.GetRadius(this.TamaKind);

			//this.SetXYSpeed(); // moved -> Ctor

			DDPicture picture = EnemyCommon.GetTamaPicture(this.TamaKind, this.TamaColor);

			for (; ; )
			{
				this.X += this.XAdd;
				this.Y += this.YAdd;

				DDDraw.DrawCenter(picture, this.X, this.Y);

				if (this.AbsorbableWeapon != -1) // ? 吸収可能
				{
					DDDraw.SetAlpha(0.5);
					DDDraw.SetBright(0.0, 0.5, 1.0);
					DDDraw.DrawBegin(Ground.I.Picture2.D_MAHOJIN_HAJIKE_00[5], this.X, this.Y);
					DDDraw.DrawRotate(DDEngine.ProcFrame / 30.0);
					DDDraw.DrawZoom(0.5);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					DDPrint.SetPrint((int)this.X, (int)this.Y);
					DDPrint.SetBorder(new I3Color(0, 0, 100));
					DDPrint.Print("[" + this.AbsorbableWeapon + "]");
					DDPrint.Reset();
				}
				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), r);

				yield return !EnemyCommon.IsEvacuated(this);
			}
		}

		protected override void Killed()
		{
			EnemyCommon.Killed(this, 0);
		}
	}
}
