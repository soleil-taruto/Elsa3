using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// へにょりレーザーの基底クラス
	/// </summary>
	public abstract class Enemy_HenyoriLaser : Enemy
	{
		private EnemyCommon_HenyoriLaser.LASER_COLOR_e Color;
		protected double Speed = 10.0;
		protected double Angle = 0.0;
		protected double Width = 20.0;
		private D2Point[] Points = new D2Point[EnemyCommon_HenyoriLaser.POINT_NUM];
		private int FirstPointIndex = 0;

		public Enemy_HenyoriLaser(double x, double y, EnemyCommon_HenyoriLaser.LASER_COLOR_e color)
			: base(x, y, Kind_e.TAMA, 0, 0)
		{
			this.Color = color;

			for (int index = 0; index < EnemyCommon_HenyoriLaser.POINT_NUM; index++)
				this.Points[index] = new D2Point(x, y);
		}

		protected override IEnumerable<bool> E_Draw()
		{
			Func<bool> a_updateParameters = SCommon.Supplier(this.E_UpdateParameters());

			for (; ; )
			{
				a_updateParameters();

				D2Point ptAdd = DDUtils.AngleToPoint(this.Angle, this.Speed);

				int prevFirstPointIndex = this.FirstPointIndex;

				this.FirstPointIndex += EnemyCommon_HenyoriLaser.POINT_NUM - 1;
				this.FirstPointIndex %= EnemyCommon_HenyoriLaser.POINT_NUM;

				this.Points[this.FirstPointIndex] = this.Points[prevFirstPointIndex];
				this.Points[this.FirstPointIndex].X += ptAdd.X;
				this.Points[this.FirstPointIndex].Y += ptAdd.Y;

				this.DrawLaser();
				this.Put当たり判定();

				yield return true;
			}
		}

		private P4Poly[] Polys = new P4Poly[EnemyCommon_HenyoriLaser.POINT_NUM - 1];

		private void DrawLaser()
		{
			for (int index = 0; index < EnemyCommon_HenyoriLaser.POINT_NUM - 1; index++)
			{
				int currPtIndex = (this.FirstPointIndex + index + 0) % EnemyCommon_HenyoriLaser.POINT_NUM;
				int prevPtIndex = (this.FirstPointIndex + index + 1) % EnemyCommon_HenyoriLaser.POINT_NUM;

				double angle = DDUtils.GetAngle(this.Points[currPtIndex] - this.Points[prevPtIndex]);
				D2Point leftWing = DDUtils.AngleToPoint(angle - Math.PI * 0.5, this.Width * 0.5);
				D2Point rightWing = leftWing * -1.0;

				this.Polys[index].LT = this.Points[currPtIndex] + leftWing;
				this.Polys[index].RT = this.Points[currPtIndex] + rightWing;
				this.Polys[index].RB = this.Points[prevPtIndex] + rightWing;
				this.Polys[index].LB = this.Points[prevPtIndex] + leftWing;
			}
			for (int index = 0; index < this.Polys.Length - 1; index++)
			{
				D2Point pt;

				pt = DDUtils.AToBRate(this.Polys[index].LB, this.Polys[index + 1].LT, 0.5);

				this.Polys[index + 0].LB = pt;
				this.Polys[index + 1].LT = pt;

				pt = DDUtils.AToBRate(this.Polys[index].RB, this.Polys[index + 1].RT, 0.5);

				this.Polys[index + 0].RB = pt;
				this.Polys[index + 1].RT = pt;
			}
			for (int index = 0; index < this.Polys.Length; index++)
			{
				DDDraw.SetIgnoreError();
				DDDraw.DrawFree(Ground.I.Picture2.D_HENYORI_LASER[(int)this.Color, index], this.Polys[index]);
				DDDraw.Reset();
			}
		}

		private void Put当たり判定()
		{
			//throw new NotImplementedException(); // TODO
		}

		/// <summary>
		/// 処理すべきこと：
		/// -- this.Speed を更新する。
		/// -- this.Angle を更新する。
		/// -- this.Width を更新する。
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<bool> E_UpdateParameters();

		protected override void Killed()
		{
			EnemyCommon.Killed(this, 0);
		}
	}
}
