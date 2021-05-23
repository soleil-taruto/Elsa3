using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Enemies.Bosses.コピーマンs
{
	public class Enemy_Boss_コピーマン : Enemy
	{
		public Enemy_Boss_コピーマン(double x, double y)
			: base(x, y, 0, 0, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// TODO

				yield return true;
			}
		}
	}
}
