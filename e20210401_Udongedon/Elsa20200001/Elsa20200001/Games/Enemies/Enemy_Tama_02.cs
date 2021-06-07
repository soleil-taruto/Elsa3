using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// シンプルな普通の敵弾
	/// </summary>
	public class Enemy_Tama_02 : Enemy
	{
		public EnemyCommon.TAMA_KIND_e TamaKind;
		public EnemyCommon.TAMA_COLOR_e TamaColor;
		public double XAdd;
		public double YAdd;

		public Enemy_Tama_02(double x, double y, double xAdd, double yAdd, EnemyCommon.TAMA_KIND_e tamaKind, EnemyCommon.TAMA_COLOR_e tamaColor)
			: base(x, y, Kind_e.TAMA, 0, 0)
		{
			// x
			// y
			// xAdd
			// yAdd

			this.TamaKind = tamaKind;
			this.TamaColor = tamaColor;
			this.XAdd = xAdd;
			this.YAdd = yAdd;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double r = EnemyCommon_Tama.GetRadius(this.TamaKind);

			DDPicture picture = EnemyCommon.GetTamaPicture(this.TamaKind, this.TamaColor);

			for (; ; )
			{
				this.X += this.XAdd;
				this.Y += this.YAdd;

				DDDraw.DrawCenter(picture, this.X, this.Y);

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
