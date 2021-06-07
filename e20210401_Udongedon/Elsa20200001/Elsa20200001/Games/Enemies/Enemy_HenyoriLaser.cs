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
		private DDPicture[,] PictureTable;
		private int PointNum;
		private EnemyCommon_HenyoriLaser.LASER_COLOR_e Color;
		private D2Point[] Points;
		private P4Poly[] Polys;

		public Enemy_HenyoriLaser(double x, double y, EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e lenKind, EnemyCommon_HenyoriLaser.LASER_COLOR_e color)
			: base(x, y, Kind_e.TAMA, 0, 0)
		{
			switch (lenKind)
			{
				case EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.SHORT:
					this.PictureTable = Ground.I.Picture2.D_HENYORI_LASER_01;
					this.PointNum = 17;
					break;

				case EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.MIDDLE:
					this.PictureTable = Ground.I.Picture2.D_HENYORI_LASER_02;
					this.PointNum = 33;
					break;

				case EnemyCommon_HenyoriLaser.LASER_LENGTH_KIND_e.LONG:
					this.PictureTable = Ground.I.Picture2.D_HENYORI_LASER_03;
					this.PointNum = 65;
					break;

				default:
					throw new DDError();
			}
			this.Color = color;
			this.Points = new D2Point[this.PointNum];
			this.Polys = new P4Poly[this.PointNum - 1];

			for (int index = 0; index < this.Points.Length; index++)
				this.Points[index] = new D2Point(x, y);
		}

		private int FirstPointIndex = 0;

		protected override IEnumerable<bool> E_Draw()
		{
			Func<bool> a_updateParameters = SCommon.Supplier(this.E_UpdateParameters());

			for (; ; )
			{
				a_updateParameters();

				D2Point ptAdd = DDUtils.AngleToPoint(this.Angle, this.Speed);

				int prevFirstPointIndex = this.FirstPointIndex;

				this.FirstPointIndex += this.PointNum - 1;
				this.FirstPointIndex %= this.PointNum;

				this.Points[this.FirstPointIndex] = this.Points[prevFirstPointIndex];
				this.Points[this.FirstPointIndex].X += ptAdd.X;
				this.Points[this.FirstPointIndex].Y += ptAdd.Y;

				this.DrawLaser();
				this.Put当たり判定();

				yield return this.Points.Any(pt => !DDUtils.IsOut(pt, new D4Rect(0, 0, GameConsts.FIELD_W, GameConsts.FIELD_H), this.Width * 0.5));
			}
		}

		private void DrawLaser()
		{
			for (int index = 0; index < this.PointNum - 1; index++)
			{
				int currPtIndex = (this.FirstPointIndex + index + 0) % this.PointNum;
				int prevPtIndex = (this.FirstPointIndex + index + 1) % this.PointNum;

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
				DDDraw.DrawFree(this.PictureTable[(int)this.Color, index], this.Polys[index]);
				DDDraw.Reset();
			}
		}

		private void Put当たり判定()
		{
			List<DDCrash> crashes = new List<DDCrash>();

			for (int index = 1; index < this.PointNum - 2; index++) // 先頭と終端は除外
			{
				int currPtIndex = (this.FirstPointIndex + index + 0) % this.PointNum;
				int prevPtIndex = (this.FirstPointIndex + index + 1) % this.PointNum;

				D2Point currPt = this.Points[currPtIndex];
				D2Point prevPt = this.Points[prevPtIndex];

				int R = Math.Max(4, (int)(this.Width / 2) - 1);

				if (index < this.PointNum / 3 || this.PointNum / 3 * 2 < index)
					R /= 2;

				int PLOT_NUM = (int)(DDUtils.GetDistance(currPt, prevPt) / R) + 1;

				for (int c = 0; c < PLOT_NUM; c++)
				{
					D2Point pt = DDUtils.AToBRate(currPt, prevPt, (double)c / PLOT_NUM);

					crashes.Add(DDCrashUtils.Circle(pt, R));
				}
			}
			this.Crash = DDCrashUtils.Multi(crashes.ToArray());
		}

		protected double Speed = 10.0;
		protected double Angle = 0.0;
		protected double Width = 20.0;

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
