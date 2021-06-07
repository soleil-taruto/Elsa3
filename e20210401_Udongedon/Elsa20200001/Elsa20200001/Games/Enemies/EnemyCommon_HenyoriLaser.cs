using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Enemies
{
	public static class EnemyCommon_HenyoriLaser
	{
		/// <summary>
		/// レーザーの長さ
		/// </summary>
		public enum LASER_LENGTH_KIND_e
		{
			SHORT = 1,
			MIDDLE,
			LONG,
		}

		/// <summary>
		/// レーザーの色
		/// </summary>
		public enum LASER_COLOR_e
		{
			RED,
			YELLOW,
			GREEN,
			CYAN,
			BLUE,
			PURPLE,
			WHITE,
		}
	}
}
