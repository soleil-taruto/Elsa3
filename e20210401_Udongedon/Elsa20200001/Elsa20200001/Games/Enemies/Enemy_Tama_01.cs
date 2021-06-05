using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Tama_01 : Enemy
	{
		private EnemyCommon.TAMA_KIND_e TamaKind;
		private EnemyCommon.TAMA_COLOR_e TamaColor;
		private double Speed;
		private double Angle;
		private Enemy Friend; // null == 無効

		public Enemy_Tama_01(double x, double y, EnemyCommon.TAMA_KIND_e tamaKind, EnemyCommon.TAMA_COLOR_e tamaColor, double speed, double angle, int absorbableWeapon = -1, Enemy friend = null)
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
			this.Friend = friend;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double r;

			switch (this.TamaKind)
			{
				case EnemyCommon.TAMA_KIND_e.NORMAL: r = 8.0; break;
				case EnemyCommon.TAMA_KIND_e.BIG: r = 12.0; break;
				case EnemyCommon.TAMA_KIND_e.LARGE: r = 30.0; break;

				// TODO: その他の敵弾についてもここへ追加..

				default:
					throw null; // never
			}
			double xAdd;
			double yAdd;

			DDUtils.MakeXYSpeed(this.X, this.Y, Game.I.Player.X, Game.I.Player.Y, this.Speed, out xAdd, out yAdd);
			DDUtils.Rotate(ref xAdd, ref yAdd, this.Angle);

			DDPicture picture = EnemyCommon.GetTamaPicture(this.TamaKind, this.TamaColor);

			for (; ; )
			{
				this.X += xAdd;
				this.Y += yAdd;

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

				if (this.Friend != null && this.Friend.HP != -1)
				{
					D2Point pt1 = new D2Point(this.X, this.Y);
					D2Point pt2 = new D2Point(this.Friend.X, this.Friend.Y);

					const int SUB_TAMA_NUM = 5;
					const double SUB_TAMA_ZOOM = 0.5;

					for (int c = 0; c < SUB_TAMA_NUM; c++)
					{
						D2Point pt = DDUtils.AToBRate(pt1, pt2, (double)c / SUB_TAMA_NUM);

						DDDraw.DrawBegin(picture, pt.X, pt.Y);
						DDDraw.DrawZoom(SUB_TAMA_ZOOM);
						DDDraw.DrawEnd();

						this.Crash = DDCrashUtils.Circle(pt, r * SUB_TAMA_ZOOM);
					}
				}
				yield return !EnemyCommon.IsEvacuated(this);
			}
		}

		protected override void Killed()
		{
			EnemyCommon.Killed(this, 0);
		}
	}
}
