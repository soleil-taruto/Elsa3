using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Novels
{
	public class ScenarioPage
	{
		public string Subtitle = "";
		public List<string> Lines = new List<string>();
		public List<ScenarioCommand> Commands = new List<ScenarioCommand>();

		// <---- prm

		public const int LINE_LEN_MAX = 59; // 要調整

		public string Text
		{
			get
			{
				return string.Join("\n", this.Lines);
			}
		}
	}
}
