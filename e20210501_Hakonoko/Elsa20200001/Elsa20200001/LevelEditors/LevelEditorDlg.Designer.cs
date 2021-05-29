namespace Charlotte.LevelEditors
{
	partial class LevelEditorDlg
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelEditorDlg));
			this.タイルGroup = new System.Windows.Forms.GroupBox();
			this.KindMember = new System.Windows.Forms.ComboBox();
			this.KindGroup = new System.Windows.Forms.ComboBox();
			this.タイルGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// タイルGroup
			// 
			this.タイルGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.タイルGroup.Controls.Add(this.KindMember);
			this.タイルGroup.Controls.Add(this.KindGroup);
			this.タイルGroup.Location = new System.Drawing.Point(12, 12);
			this.タイルGroup.Name = "タイルGroup";
			this.タイルGroup.Size = new System.Drawing.Size(360, 110);
			this.タイルGroup.TabIndex = 0;
			this.タイルGroup.TabStop = false;
			this.タイルGroup.Text = "タイル";
			this.タイルGroup.Enter += new System.EventHandler(this.タイルGroup_Enter);
			// 
			// KindMember
			// 
			this.KindMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.KindMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.KindMember.FormattingEnabled = true;
			this.KindMember.Location = new System.Drawing.Point(6, 60);
			this.KindMember.Name = "KindMember";
			this.KindMember.Size = new System.Drawing.Size(348, 28);
			this.KindMember.TabIndex = 1;
			this.KindMember.SelectedIndexChanged += new System.EventHandler(this.KindMember_SelectedIndexChanged);
			// 
			// KindGroup
			// 
			this.KindGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.KindGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.KindGroup.FormattingEnabled = true;
			this.KindGroup.Location = new System.Drawing.Point(6, 26);
			this.KindGroup.Name = "KindGroup";
			this.KindGroup.Size = new System.Drawing.Size(348, 28);
			this.KindGroup.TabIndex = 0;
			this.KindGroup.SelectedIndexChanged += new System.EventHandler(this.KindGroup_SelectedIndexChanged);
			// 
			// LevelEditorDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 161);
			this.Controls.Add(this.タイルGroup);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LevelEditorDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Editor";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.LevelEditorDlg_Load);
			this.Shown += new System.EventHandler(this.LevelEditorDlg_Shown);
			this.タイルGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox タイルGroup;
		private System.Windows.Forms.ComboBox KindGroup;
		private System.Windows.Forms.ComboBox KindMember;
	}
}