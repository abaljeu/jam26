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
        native = new Bitmap(320, 240);
    }

    protected override void OnPaint(PaintEventArgs e)
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




  