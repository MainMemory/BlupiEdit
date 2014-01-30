using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace BlupiEdit
{
	public partial class LevelSelectForm : Form
	{
		public LevelSelectForm()
		{
			InitializeComponent();
		}

		List<int> users = new List<int>(8) { 0 };
		List<int> levels;

		private void LevelSelectForm_Load(object sender, EventArgs e)
		{
			levelSetList.BeginUpdate();
			for (int i = 1; i < 9; i++)
				if (File.Exists(string.Format("data\\info{0:000}.blp", i)))
				{
					users.Add(i);
					using (FileStream str = File.OpenRead(string.Format("data\\info{0:000}.blp", i)))
					using (BinaryReader r = new BinaryReader(str))
					{
						str.Seek(0x16, SeekOrigin.Begin);
						levelSetList.Items.Add("User " + i + " - " + r.ReadString(40));
					}
				}
			levelSetList.EndUpdate();
			levelSetList.SelectedIndex = 0;
		}

		private void levelSetList_SelectedIndexChanged(object sender, EventArgs e)
		{
			int max = levelSetList.SelectedIndex == 0 ? 399 : 20;
			numericUpDown1.Maximum = max;
			levels = new List<int>();
			levelList.BeginUpdate();
			levelList.Items.Clear();
			for (int i = 1; i <= max; i++)
			{
				string s = LevelData.GetLevelName(users[levelSetList.SelectedIndex], i);
				if (File.Exists(s))
				{
					levels.Add(i);
					using (FileStream str = File.OpenRead(s))
					using (BinaryReader r = new BinaryReader(str))
					{
						str.Seek(0x178, SeekOrigin.Begin);
						string n = r.ReadString(40);
						levelList.Items.Add("#" + i + (string.IsNullOrEmpty(n) ? "" : " - " + n));
					}
				}
			}
			levelList.EndUpdate();
		}

		private void levelList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (levelList.SelectedIndex == -1)
				pictureBox1.Image = null;
			else
			{
				int imgsize = LevelData.GridSize * 8;
				Bitmap bmp = new Bitmap(imgsize, imgsize);
				Graphics gfx = Graphics.FromImage(bmp);
				gfx.CompositingQuality = CompositingQuality.HighSpeed;
				gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
				gfx.PixelOffsetMode = PixelOffsetMode.None;
				gfx.SmoothingMode = SmoothingMode.None;
				LevelData level = new LevelData(LevelData.GetLevelName(users[levelSetList.SelectedIndex], levels[levelList.SelectedIndex]));
				int startx = level.HorizontalScroll ? Math.Max(Math.Min(level.StartPositions[0].X - (imgsize / 2), LevelData.PixelSize - imgsize), 0) : 0;
				int starty = level.VerticalScroll ? Math.Max(Math.Min(level.StartPositions[0].Y - (imgsize / 2), LevelData.PixelSize - imgsize), 0) : 0;
				using (Bitmap bg = LevelData.LoadImage(string.Format("decor{0:000}", level.Background)))
					for (int x = -startx / 2; x < bmp.Width; x += 640)
						for (int y = -starty / 2; y < bmp.Height; y += 480)
							gfx.DrawImage(bg, x, y, bg.Width, bg.Height);
				gfx.DrawSprite(LevelData.TileImages[TileTypes.Blupi000][level.StartDirections[0] ? 0 : 1],
					level.StartPositions[0] - new Size(startx, starty));
				foreach (LevelItem item in level.Items)
					gfx.DrawSprite(LevelData.TileImages[item.ArtFile][item.Tile],
						item.PointA - new Size(startx, starty));
				for (int y = Math.Max(starty / LevelData.GridSize, 0); y < Math.Min((starty + imgsize), LevelData.PixelSize) / LevelData.GridSize; y++)
					for (int x = Math.Max(startx / LevelData.GridSize, 0); x < Math.Min((startx + imgsize), LevelData.PixelSize) / LevelData.GridSize; x++)
						if (level.Tiles[x, y] != -1)
							gfx.DrawSprite(LevelData.TileImages[TileTypes.Object][level.Tiles[x, y]],
								new Point(x * LevelData.GridSize - startx, y * LevelData.GridSize - starty));
				pictureBox1.Image = bmp;
				numericUpDown1.Value = levels[levelList.SelectedIndex];
			}
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if ((levelList.SelectedIndex = levels.IndexOf((int)numericUpDown1.Value)) == -1)
				openButton.Text = "&New";
			else
				openButton.Text = "&Open";
		}

		public int UserID { get { return users[levelSetList.SelectedIndex]; } }

		public int LevelNum { get { return (int)numericUpDown1.Value; } }
	}
}