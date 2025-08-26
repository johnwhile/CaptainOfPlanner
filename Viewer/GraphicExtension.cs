
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    internal static class GraphicExtension
    {
        public static GraphicsPath RoundedRectanglePath(int x, int y, int width, int height, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
        public static bool IsMouseOver(this Control control, Point screen)
        {
            Point local = control.PointToClient(screen);
            return !(
                local.X < 0 || 
                local.Y < 0 ||
                local.X > control.Width ||
                local.Y > control.Height);
        }

        public static Point Sum(this Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);
        public static Point Sub(this Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);
    }
}
