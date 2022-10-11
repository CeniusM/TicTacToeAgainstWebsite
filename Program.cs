using System.Drawing;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
Rectangle bounds = Screen.GetBounds(Point.Empty);

Bitmap GetSreenshot()
{
  Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
  Graphics g = Graphics.FromImage(bm);
  g.CopyFromScreen(0, 0, 0, 0, bm.Size);
  return bm;
}


using(Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
{
    using(Graphics g = Graphics.FromImage(bitmap))
    {
         g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
    }
    
    bitmap.Save("test.png");
}