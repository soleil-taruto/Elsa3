using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public static class GameConsts
	{
		public const int TILE_W = 32;
		public const int TILE_H = 32;

		public static int PLAYER_DEAD_FRAME_MAX { get { return Game.I != null && Game.I.FinalZone != null ? 200 : 40; } }
		public const int PLAYER_REBORN_FRAME_MAX = 30;
	}
}
