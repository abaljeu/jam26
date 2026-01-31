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
            gdiControl1 = new GdiControl();
            SuspendLayout();
            // 
            // gdiControl1
            // 
            gdiControl1.BackColor = Color.White;
            gdiControl1.Dock = DockStyle.Fill;
            gdiControl1.Location = new Point(0, 0);
            gdiControl1.Name = "gdiControl1";
            gdiControl1.Size = new Size(798, 454);
            gdiControl1.TabIndex = 0;
            gdiControl1.Text = "gdiControl1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 454);
            Controls.Add(gdiControl1);
            Name = "Form1";
            Text = "Escape from Montreal";
            ResumeLayout(false);
        }

        #endregion

        private GdiControl gdiControl1;
  }
}
