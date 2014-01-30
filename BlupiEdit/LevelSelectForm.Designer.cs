namespace BlupiEdit
{
    partial class LevelSelectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.levelSetList = new System.Windows.Forms.ListBox();
			this.levelList = new System.Windows.Forms.ListBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.openButton = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// levelSetList
			// 
			this.levelSetList.Items.AddRange(new object[] {
            "Worlds"});
			this.levelSetList.Location = new System.Drawing.Point(12, 12);
			this.levelSetList.Name = "levelSetList";
			this.levelSetList.Size = new System.Drawing.Size(120, 121);
			this.levelSetList.TabIndex = 0;
			this.levelSetList.SelectedIndexChanged += new System.EventHandler(this.levelSetList_SelectedIndexChanged);
			// 
			// levelList
			// 
			this.levelList.FormattingEnabled = true;
			this.levelList.Location = new System.Drawing.Point(138, 12);
			this.levelList.Name = "levelList";
			this.levelList.Size = new System.Drawing.Size(152, 173);
			this.levelList.TabIndex = 1;
			this.levelList.SelectedIndexChanged += new System.EventHandler(this.levelList_SelectedIndexChanged);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(215, 227);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// openButton
			// 
			this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.openButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.openButton.Location = new System.Drawing.Point(134, 227);
			this.openButton.Name = "openButton";
			this.openButton.Size = new System.Drawing.Size(75, 23);
			this.openButton.TabIndex = 3;
			this.openButton.Text = "&Open";
			this.openButton.UseVisualStyleBackColor = true;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(138, 192);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            399,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
			this.numericUpDown1.TabIndex = 5;
			this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(296, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(238, 238);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// LevelSelectForm
			// 
			this.AcceptButton = this.openButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(546, 262);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.openButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.levelList);
			this.Controls.Add(this.levelSetList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LevelSelectForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Select a level...";
			this.Load += new System.EventHandler(this.LevelSelectForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox levelSetList;
        private System.Windows.Forms.ListBox levelList;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}