using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Designs;

namespace Charlotte.Games
{
	public class Map
	{
		public string MapFile;
		public Design Design;

		public Map(string mapFile, Design design, uint seed)
		{
			this.MapFile = mapFile;
			this.Design = design;
			DDUtils.Random = new DDRandom(seed);
			this.Load();
		}

		public MapCell[,] Table; // 添字：[x,y]
		public int W;
		public int H;
		public string WallName;
		public string MusicName;

		public void Load()
		{
			I3Color[,] bmp = Common.ReadBmpFile(DDResource.Load(this.MapFile), out this.W, out this.H);

			this.Table = new MapCell[this.W, this.H];

			for (int x = 0; x < this.W; x++)
			{
				for (int y = 0; y < this.H; y++)
				{
					int index = SCommon.IndexOf(MapCell.Kind_e_Colors, v => v == bmp[x, y]);

#if true
					if (index == -1) // ? 未知の色
						index = (int)MapCell.Kind_e.EMPTY;
#else
					// @ 2020.12.x
					// ペイントなどで編集して不明な色が混入してしまうことを考慮して、EMPTY に矯正するようにしたいが、
					// 移植によるバグ混入検出のため、当面はエラーで落とす。
					// -- 止め @ 2021.1.x
					//
					if (index == -1) // ? 未知の色
						throw null;
#endif

					this.Table[x, y] = new MapCell()
					{
						Parent = this,
						Self_X = x,
						Self_Y = y,
						Kind = (MapCell.Kind_e)index,
						ColorPhase = DDUtils.Random.Real(),
					};
				}
			}
		}

		public void Save()
		{
			I3Color[,] bmp = new I3Color[this.W, this.H];

			for (int x = 0; x < this.W; x++)
				for (int y = 0; y < this.H; y++)
					bmp[x, y] = MapCell.Kind_e_Colors[(int)this.Table[x, y].Kind];

			DDResource.Save(this.MapFile, Common.WriteBmpFile(bmp, this.W, this.H));
		}

		public MapCell GetCell(I2Point pt)
		{
			return this.GetCell(pt.X, pt.Y);
		}

		public MapCell GetCell(int x, int y)
		{
			if (
				x < 0 || this.W <= x ||
				y < 0 || this.H <= y
				)
				return GameCommon.DefaultMapCell;

			return this.Table[x, y];
		}

		public bool FindCell(out int x, out int y, Predicate<MapCell> match)
		{
			for (x = 0; x < this.W; x++)
				for (y = 0; y < this.H; y++)
					if (match(this.Table[x, y]))
						return true;

			x = -1;
			y = -1;

			return false;
		}
	}
}
