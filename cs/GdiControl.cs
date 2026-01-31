using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//tremnonal
namespace mask
{
    // idea: class Tile.  draws itself and border, using bitmap image.  maybe applies mask filter, or maybe image does and we use it.
    public class GdiControl : Control
    {
        Font debugFont;
        Brush debugBrush;

        Bitmap reference;
        Bitmap pointer;

        Bitmap native;

        bool imagesLoaded;

        int scaleFactor = 3;
        int mouseX, mouseY;



        public GdiControl()
        {
            debugFont = new Font("Arial", 16);
            debugBrush = new SolidBrush(Color.White);
            this.DoubleBuffered = true;
            Cursor.Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawString("Placeholder because otherwise the Visual Studio Forms\n designer will take a lock on your bitmaps.", debugFont, debugBrush, 0, 0);
                return;
            }

            if (!imagesLoaded)
            {
                reference = new Bitmap("D:\\repos\\jam26\\images\\Reference.png");
                reference.SetResolution(96, 96);

                pointer = new Bitmap("D:\\repos\\jam26\\images\\Pointer.png");
                pointer.SetResolution(96, 96);

                native = new Bitmap(320, 240);
                native.SetResolution(96, 96);

                imagesLoaded = true;
            }
            {
                Graphics g = Graphics.FromImage(native);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImageUnscaled(reference, 0, 0, 320, 240);

                // Draw the mouse pointer
                g.DrawImageUnscaled(pointer, mouseX, mouseY);

            }

            // Draw native to final target with 3x scaling
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(Color.Black);
            e.Graphics.DrawImage(native, 0, 0, 320 * scaleFactor, 240 * scaleFactor);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseX = e.X / scaleFactor;
            mouseY = e.Y / scaleFactor;
            this.Invalidate();
        }
    }
}




