using System;
using System.Drawing;
using System.Windows.Forms;

//tremnonal
namespace mask
{
  // idea: class Tile.  draws itself and border, using bitmap image.  maybe applies mask filter, or maybe image does and we use it.
  public class GdiControl : Control
  {
    
    public GdiControl()
    {
      SetStyle(ControlStyles.AllPaintingInWmPaint
        | ControlStyles.OptimizedDoubleBuffer
        | ControlStyles.ResizeRedraw, true);
      BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      using var pen = new Pen(Color.Blue, 2);
      var gr = e.Graphics;
      gr.DrawRectangle(pen, new Rectangle(10, 10, Width-20, Height-20));
    }

  }
}




  