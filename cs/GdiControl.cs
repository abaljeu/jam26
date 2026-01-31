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
        Bitmap uiflavor;
        Bitmap glyphs;

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

        void DrawMessageBoxText(Graphics g)
        {
            // Draw message box content
            string text = "Hello, Hackforge!";

            int dstX = 52;
            int w = 8;
            int h = 16;

            for (int i=0; i<text.Length; ++i)
            {
                Rectangle destRect = new Rectangle(dstX, 197, w, h);

                char glyph = text[i];

                int srcX = -1;

                if (glyph >= 'A' && glyph <= 'Z')
                {
                    srcX = (glyph - 65) * 8;
                }
                else if (glyph >= 'a' && glyph <= 'z')
                {
                    srcX = (glyph - 97 + 26) * 8;
                }
                else if (glyph == ',')
                {
                    srcX = 52 * 8;
                }
                else if (glyph == '.')
                {
                    srcX = 53 * 8;
                }
                else if (glyph == ':')
                {
                    srcX = 54 * 8;
                }
                else if (glyph == ';')
                {
                    srcX = 55 * 8;
                }
                else if (glyph == '\'')
                {
                    srcX = 56 * 8;
                }
                else if (glyph >= '0' && glyph <= '9')
                {
                    srcX = (glyph - 48 + 57) * 8;
                }
                else if (glyph == '!')
                {
                    srcX = 67 * 8;
                }

                if (srcX > 0)
                {
                    Rectangle sourceRect = new Rectangle(srcX, 0, w, h);

                    g.DrawImage(glyphs, destRect, sourceRect, GraphicsUnit.Pixel);
                }

                dstX += w;
            }
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

                uiflavor = new Bitmap("D:\\repos\\jam26\\images\\UI.png");
                uiflavor.SetResolution(96, 96);

                glyphs = new Bitmap("D:\\repos\\jam26\\images\\glyphs8x16.png");
                glyphs.SetResolution(96, 96);

                native = new Bitmap(320, 240);
                native.SetResolution(96, 96);

                imagesLoaded = true;
            }
            {
                Graphics g = Graphics.FromImage(native);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImageUnscaled(reference, 0, 0, 320, 240);

                DrawMessageBoxText(g);

                // Draw the HUD
                g.DrawImageUnscaled(uiflavor, 0, 0);

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




