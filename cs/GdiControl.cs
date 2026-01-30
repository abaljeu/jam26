using System;
using System.Drawing;
using System.Windows.Forms;

//tremnonal
namespace mask
{
    // idea: class Tile.  draws itself and border, using bitmap image.  maybe applies mask filter, or maybe image does and we use it.
    public class GdiControl : Control
    {
        Bitmap reference;
        Bitmap native;

        public GdiControl()
        {
            reference = new Bitmap("D:\\repos\\jam26\\images\\Reference.png");
            reference.SetResolution(96, 96);

            native = new Bitmap(320, 240);
            native.SetResolution(96, 96);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            {
                Graphics g = Graphics.FromImage(native);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImageUnscaled(reference, 0, 0, 320, 240);
            }

            // Draw native to final target with 2x scaling
            int scaleFactor = 3;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(native, 0, 0, 320 * scaleFactor, 240 * scaleFactor);
        }

    }
}




