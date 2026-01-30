
using System.Drawing.Drawing2D;

namespace mask
{
    public partial class Form1 : Form
    {
        Bitmap reference;
        Bitmap native;

        public Form1()
        {
            InitializeComponent();
            reference = new Bitmap("D:\\repos\\jam26\\images\\Reference.png");
            native = new Bitmap(320, 240);
        }

        private void gdiControl1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void gdiControl1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void gdiControl1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void gdiControl1_Paint(object sender, PaintEventArgs e)
        {
            {
                Graphics g = Graphics.FromImage(native);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.DrawImageUnscaled(reference, 0, 0);
            }

            // Draw native to final target
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.DrawImageUnscaled(native, 0, 0);
        }
    }
}
