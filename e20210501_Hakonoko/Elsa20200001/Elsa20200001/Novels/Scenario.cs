using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels
{
	public class Scenario
	{
		public const string SCENARIO_FILE_PREFIX = "res\\Scenario\\";
		public const string SCENARIO_FILE_SUFFIX = ".txt";

		public string Name;
		public List<ScenarioPage> Pages = new List<ScenarioPage>();

		public Scenario(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new DDError();

			this.Name = name;
			this.Pages.Clear();

			string[] lines = this.ReadScenarioLines(name);
			ScenarioPage page = null;

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index].Trim();

				if (line == "")
					continue;

				if (line[0] == '#') // ? 外部ファイル参照
				{
					string subName = line.Substring(1);
					string[] subLines = this.ReadScenarioLines(subName);

					lines = lines.Take(index).Concat(subLines).Concat(lines.Skip(index + 1)).ToArray();

					// HACK: このへん要調整, 問題ないか要チェック
				}
			}

			{
				Dictionary<string, string> def_dic = SCommon.CreateDictionary<string>();

				for (int index = 0; index < lines.Length; index++)
				{
					string line = lines[index].Trim();

					if (line == "")
						continue;

					if (line[0] == '^') // ? 定義
					{
						line = line.Substring(1); // ^ 除去

						string[] tokens = SCommon.Tokenize(line, " ", false, true, 2);
						string def_name = tokens[0];
						string def_value = tokens[1];

						def_dic.Add(def_name, def_value);

						lines[index] = "";
					}
				}
				for (int index = 0; index < lines.Length; index++)
				{
					string line = lines[index];

					foreach (KeyValuePair<string, string> pair in def_dic)
						line = line.Replace(pair.Key, pair.Value);

					lines[index] = line;
				}
			}

			foreach (string f_line in lines)
			{
				string line = f_line.Trim();

				if (line == "")
					continue;

				if (line[0] == ';') // ? コメント行
					continue;

				if (line[0] == '/')
				{
					page = new ScenarioPage()
					{
						Subtitle = line.Substring(1)
					};

					this.Pages.Add(page);
				}
				else if (page == null)
				{
					throw new DDError("シナリオの先頭は /xxx でなければなりません。");
				}
				else if (line[0] == '!') // ? コマンド
				{
					string[] tokens = line.Substring(1).Split(' ').Where(v => v != "").ToArray();

					page.Commands.Add(new ScenarioCommand(tokens));
				}
				else
				{
					page.Lines.Add(line);
				}
			}
			this.各ページの各行の長さ調整();
		}

		private string[] ReadScenarioLines(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new DDError();

			byte[] fileData;

			{
				const string DEVENV_SCENARIO_DIR = "シナリオデータ";
				const string DEVENV_SCENARIO_SUFFIX = ".txt";

				if (Directory.Exists(DEVENV_SCENARIO_DIR))
				{
					string file = Path.Combine(DEVENV_SCENARIO_DIR, name + DEVENV_SCENARIO_SUFFIX);

					fileData = File.ReadAllBytes(file);
				}
				else
				{
					string file = SCENARIO_FILE_PREFIX + name + SCENARIO_FILE_SUFFIX;

					fileData = DDResource.Load(file);
				}
			}

			string text = SCommon.ToJString(fileData, true, true, true, true);

			text = text.Replace('\t', ' '); // タブスペースと空白 -> 空白に統一

			string[] lines = SCommon.TextToLines(text);
			return lines;
		}

		private void 各ページの各行の長さ調整()
		{
			foreach (ScenarioPage page in this.Pages)
			{
				for (int index = 0; index < page.Lines.Count; index++)
				{
					if (ScenarioPage.LINE_LEN_MAX < page.Lines[index].Length)
					{
						page.Lines.Insert(index + 1, page.Lines[index].Substring(ScenarioPage.LINE_LEN_MAX));
						page.Lines[index] = page.Lines[index].Substring(0, ScenarioPage.LINE_LEN_MAX);
					}
				}
			}
		}
	}
}
