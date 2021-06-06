using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Walls
{
	public class Wall_32001_背面 : Wall
	{
		protected override IEnumerable<bool> E_Draw()
		{
			return WallCommon.Standard(this, Ground.I.Picture.P_WALL_32001_背面, 0, 3, 0, 0, 0.01, 1.0, true, 0.5);
		}
	}
}
