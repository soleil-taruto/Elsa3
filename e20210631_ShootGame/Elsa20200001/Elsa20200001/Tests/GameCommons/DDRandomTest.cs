using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.GameCommons;

namespace Charlotte.Tests.GameCommons
{
	public class DDRandomTest
	{
		public void Test01()
		{
			DDRandom random = new DDRandom(0u);

			using (StreamWriter writer = new StreamWriter(@"C:\temp\1000000-rand.txt", false, Encoding.ASCII))
			{
				for (int c = 0; c < 1000000; c++)
				{
					uint value = random.Next();

					writer.WriteLine(Common.ZPad(Convert.ToString(value, 2), 32) + "\t" + value.ToString("x8") + "\t" + Common.ZPad("" + value, 10));
				}
			}
		}
	}
}
