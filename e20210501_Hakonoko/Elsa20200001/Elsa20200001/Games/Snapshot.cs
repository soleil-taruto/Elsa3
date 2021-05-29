using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games.Enemies;

namespace Charlotte.Games
{
	public class Snapshot
	{
		public D2Point PlayerPosition;
		public D2Point PlayerVelocity;
		public int[] PlayerOtherStatus;
		public List<Enemy> Enemies;
	}
}
