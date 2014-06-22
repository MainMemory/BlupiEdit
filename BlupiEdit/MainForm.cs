using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BlupiEdit
{
	public partial class MainForm : Form
	{
		int userid, levelnum;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (Program.Arguments.Length > 0)
				LoadGame(Program.Arguments[0]);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (LevelData.CurrentLevel != null)
				switch (MessageBox.Show(this, "Do you want to save?", "BlupiEdit", MessageBoxButtons.YesNoCancel))
				{
					case DialogResult.Yes:
						LevelData.SaveLevel();
						break;
					case DialogResult.Cancel:
						e.Cancel = true;
						break;
				}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog fd = new OpenFileDialog() { DefaultExt = "exe", Filter = "EXE Files|*.exe", RestoreDirectory = true })
				if (fd.ShowDialog(this) == DialogResult.OK)
					LoadGame(fd.FileName);
		}

		private void LoadGame(string filename)
		{
			LevelData.LoadGame(filename);
			changeLevelToolStripMenuItem.Enabled = true;
		}

		private void changeLevelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (LevelSelectForm ls = new LevelSelectForm(LevelSelectFormMode.Open))
				if (ls.ShowDialog(this) == DialogResult.OK)
				{
					userid = ls.UserID;
					levelnum = ls.LevelNum;
					LevelData.LoadLevel(userid, levelnum);
					// TODO: more stuff
					UpdateFormText();
					saveAsToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = true;
				}
		}

		private void UpdateFormText()
		{
			StringBuilder sb = new StringBuilder("BlupiEdit - Speedy Blupi");
			if (LevelData.IsBlupi2) sb.Append(" 2");
			sb.Append(" - ");
			if (userid == 0)
				sb.AppendFormat("World {0:000}", levelnum);
			else
				sb.AppendFormat("User {0} Design {1:000}", userid, levelnum);
			if (!string.IsNullOrEmpty(LevelData.CurrentLevel.LevelName))
				sb.AppendFormat(" - {0}", LevelData.CurrentLevel.LevelName);
			Text = sb.ToString();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LevelData.SaveLevel();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (LevelSelectForm ls = new LevelSelectForm(LevelSelectFormMode.Save))
				if (ls.ShowDialog(this) == DialogResult.OK)
				{
					userid = ls.UserID;
					levelnum = ls.LevelNum;
					LevelData.ChangeLevelPath(userid, levelnum);
					LevelData.SaveLevel();
					UpdateFormText();
				}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}