using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Enemies
{
	public static class EnemyCommon_Tama
	{
		public static double GetRadius(EnemyCommon.TAMA_KIND_e kind)
		{
			double r;

			switch (kind)
			{
				case EnemyCommon.TAMA_KIND_e.SMALL: r = 4.0; break;
				case EnemyCommon.TAMA_KIND_e.NORMAL: r = 8.0; break;
				case EnemyCommon.TAMA_KIND_e.DOUBLE: r = 8.0; break;
				case EnemyCommon.TAMA_KIND_e.BIG: r = 12.0; break;
				case EnemyCommon.TAMA_KIND_e.LARGE: r = 30.0; break;
				case EnemyCommon.TAMA_KIND_e.KNIFE: r = 6.0; break;

				// TODO: その他の敵弾についてもここへ追加..

				default:
					throw null; // never
			}
			return r;
		}
	}
}
