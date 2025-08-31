using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class GraphicalOverlay : UserControl
    {
        Matrix rotation = new Matrix();
        Control Overlaied;
       
/*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }
*/
        public GraphicalOverlay(Control ToOverlay)
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            //SetStyle(ControlStyles.Opaque, true);
            //BackColor = Color.Transparent;
            BackColor = Color.FromArgb(100, 100, 100, 100);
            Enabled = false;
            Overlaied = ToOverlay;
            Size = Overlaied.Size;

            Point location = new Point()
            {
                X = Overlaied.Location.X + 20,
                Y = Overlaied.Location.Y + 20
            };

            Location = location;
            Size = new Size(500, 500);
            //Dock = DockStyle.Fill;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clear(BackColor);
            rotation.RotateAt(1, new PointF(Width / 2f, Height / 2f));
            e.Graphics.Transform = rotation;
            e.Graphics.DrawLine(Pens.Red, 0, 0, Width, Height);
            e.Graphics.Transform.Reset();
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
