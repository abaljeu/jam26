namespace mask
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      Label labelScore;
      gdiControl1 = new GdiControl();
      panelTop = new Panel();
      menuStrip1 = new MenuStrip();
      fileMenu = new ToolStripMenuItem();
      newGameMenu = new ToolStripMenuItem();
      exitMenu = new ToolStripMenuItem();
      editMenu = new ToolStripMenuItem();
      helpMenu = new ToolStripMenuItem();
      panelBottom = new Panel();
      panelLeft = new Panel();
      textBox1 = new TextBox();
      labelMasks = new Label();
      labelScore = new Label();
      panelTop.SuspendLayout();
      menuStrip1.SuspendLayout();
      panelLeft.SuspendLayout();
      SuspendLayout();
      // 
      // gdiControl1
      // 
      gdiControl1.BackColor = Color.White;
      gdiControl1.Dock = DockStyle.Fill;
      gdiControl1.Location = new Point(85, 67);
      gdiControl1.Name = "gdiControl1";
      gdiControl1.Size = new Size(715, 351);
      gdiControl1.TabIndex = 0;
      gdiControl1.Text = "gdiControl1";
      // 
      // panelTop
      // 
      panelTop.BackColor = SystemColors.ControlDark;
      panelTop.BorderStyle = BorderStyle.FixedSingle;
      panelTop.Controls.Add(labelScore);
      panelTop.Controls.Add(textBox1);
      panelTop.Controls.Add(menuStrip1);
      panelTop.Dock = DockStyle.Top;
      panelTop.Location = new Point(0, 0);
      panelTop.Name = "panelTop";
      panelTop.Size = new Size(800, 67);
      panelTop.TabIndex = 3;
      // 
      // menuStrip1
      // 
      menuStrip1.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      menuStrip1.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, helpMenu });
      menuStrip1.Location = new Point(0, 0);
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Size = new Size(798, 24);
      menuStrip1.TabIndex = 0;
      menuStrip1.Text = "&File";
      // 
      // fileMenu
      // 
      fileMenu.DropDownItems.AddRange(new ToolStripItem[] { newGameMenu, exitMenu });
      fileMenu.Name = "fileMenu";
      fileMenu.Size = new Size(40, 20);
      fileMenu.Text = "&File";
      // 
      // newGameMenu
      // 
      newGameMenu.Name = "newGameMenu";
      newGameMenu.Size = new Size(138, 22);
      newGameMenu.Text = "&New Game";
      // 
      // exitMenu
      // 
      exitMenu.Name = "exitMenu";
      exitMenu.Size = new Size(138, 22);
      exitMenu.Text = "E&xit";
      // 
      // editMenu
      // 
      editMenu.Name = "editMenu";
      editMenu.Size = new Size(42, 20);
      editMenu.Text = "&Edit";
      // 
      // helpMenu
      // 
      helpMenu.Name = "helpMenu";
      helpMenu.Size = new Size(45, 20);
      helpMenu.Text = "&Help";
      // 
      // panelBottom
      // 
      panelBottom.BackColor = SystemColors.ControlDark;
      panelBottom.BorderStyle = BorderStyle.FixedSingle;
      panelBottom.Dock = DockStyle.Bottom;
      panelBottom.Location = new Point(0, 418);
      panelBottom.Name = "panelBottom";
      panelBottom.Size = new Size(800, 32);
      panelBottom.TabIndex = 5;
      // 
      // panelLeft
      // 
      panelLeft.BackColor = SystemColors.ControlDark;
      panelLeft.BorderStyle = BorderStyle.FixedSingle;
      panelLeft.Controls.Add(labelMasks);
      panelLeft.Dock = DockStyle.Left;
      panelLeft.Location = new Point(0, 67);
      panelLeft.Name = "panelLeft";
      panelLeft.Size = new Size(85, 351);
      panelLeft.TabIndex = 6;
      // 
      // textBox1
      // 
      textBox1.Location = new Point(53, 34);
      textBox1.Name = "textBox1";
      textBox1.Size = new Size(100, 23);
      textBox1.TabIndex = 1;
      // 
      // labelScore
      // 
      labelScore.AutoSize = true;
      labelScore.Location = new Point(11, 34);
      labelScore.Name = "labelScore";
      labelScore.Size = new Size(36, 15);
      labelScore.TabIndex = 2;
      labelScore.Text = "Score";
      // 
      // labelMasks
      // 
      labelMasks.AutoSize = true;
      labelMasks.Location = new Point(21, 105);
      labelMasks.Name = "labelMasks";
      labelMasks.Size = new Size(40, 15);
      labelMasks.TabIndex = 1;
      labelMasks.Text = "Masks";
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(gdiControl1);
      Controls.Add(panelLeft);
      Controls.Add(panelBottom);
      Controls.Add(panelTop);
      MainMenuStrip = menuStrip1;
      Name = "Form1";
      Text = "Escape from Montreal";
      panelTop.ResumeLayout(false);
      panelTop.PerformLayout();
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      panelLeft.ResumeLayout(false);
      panelLeft.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private GdiControl gdiControl1;
    private Panel panelTop;
    private MenuStrip menuStrip1;
    private Panel panelBottom;
    private ToolStripMenuItem fileMenu;
    private ToolStripMenuItem newGameMenu;
    private ToolStripMenuItem exitMenu;
    private ToolStripMenuItem editMenu;
    private ToolStripMenuItem helpMenu;
    private Panel panelLeft;
    private TextBox textBox1;
    private Label labelMasks;
  }
}
