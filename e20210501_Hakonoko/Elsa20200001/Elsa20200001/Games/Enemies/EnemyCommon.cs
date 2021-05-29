using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public static class EnemyCommon
	{
		public static bool IsOutOfScreen_ForDraw(Enemy enemy)
		{
			return IsOutOfScreen_ForDraw(new D2Point(enemy.X - DDGround.Camera.X, enemy.Y - DDGround.Camera.Y));
		}

		public static bool IsOutOfScreen_ForDraw(D2Point pt)
		{
			return DDUtils.IsOutOfScreen(pt, EnemyConsts.OUT_OF_SCREEN_MARGIN);
		}
	}
}
