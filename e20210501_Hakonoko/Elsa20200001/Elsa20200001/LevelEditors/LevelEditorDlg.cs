using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using Charlotte.Games.Enemies;
using Charlotte.Games;

namespace Charlotte.LevelEditors
{
	public partial class LevelEditorDlg : Form
	{
		#region ALT_F4 抑止

		public bool XPressed = false;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				this.XPressed = true;
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public LevelEditorDlg()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void LevelEditorDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void LevelEditorDlg_Shown(object sender, EventArgs e)
		{
			this.KindGroup.Items.Clear();

			foreach (LevelEditor.GroupInfo tileGroup in LevelEditor.KindGroups)
				this.KindGroup.Items.Add(tileGroup.Name);

			P_PostSetItems(this.KindGroup);
		}

		private void P_PostSetItems(
			ComboBox // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				combo)
		{
			combo.SelectedIndex = 0;
			combo.MaxDropDownItems = Math.Min(combo.Items.Count, 100);
		}

		public MapCell.Kind_e GetKind()
		{
			return (MapCell.Kind_e)LevelEditor.KindGroups[this.KindGroup.SelectedIndex].Members[this.KindMember.SelectedIndex].Index;
		}

		public void SetKind(MapCell.Kind_e kind)
		{
			for (int groupIndex = 0; groupIndex < LevelEditor.KindGroups.Count; groupIndex++)
			{
				for (int memberIndex = 0; memberIndex < LevelEditor.KindGroups[groupIndex].Members.Count; memberIndex++)
				{
					if (LevelEditor.KindGroups[groupIndex].Members[memberIndex].Index == (int)kind)
					{
						this.KindGroup.SelectedIndex = groupIndex;
						this.KindMember.SelectedIndex = memberIndex;
						return;
					}
				}
			}
		}

		private void タイルGroup_Enter(object sender, EventArgs e)
		{
			// noop
		}

		private void KindGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.KindMember.Items.Clear();

			foreach (LevelEditor.GroupInfo.MemberInfo tileMember in LevelEditor.KindGroups[this.KindGroup.SelectedIndex].Members)
				this.KindMember.Items.Add(tileMember.Name);

			P_PostSetItems(this.KindMember);
		}

		private void KindMember_SelectedIndexChanged(object sender, EventArgs e)
		{
			// noop
		}
	}
}
