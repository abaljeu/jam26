
namespace mask
{
    public partial class Form1 : Form
    {
        Bitmap reference;

        public Form1()
        {
            InitializeComponent();
            reference = new Bitmap("D:\\repos\\jam26\\images\\Reference.png");
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
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.DrawImageUnscaled(reference, 0, 0);
        }
    }
}
