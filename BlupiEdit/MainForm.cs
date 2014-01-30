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


		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (Program.Arguments.Length > 0)
				LoadGame(Program.Arguments[0]);
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{

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
			using (LevelSelectForm ls = new LevelSelectForm())
				if (ls.ShowDialog(this) == DialogResult.OK)
				{
					LevelData.LoadLevel(ls.UserID, ls.LevelNum);
					// TODO: more stuff
				}
		}
	}
}
