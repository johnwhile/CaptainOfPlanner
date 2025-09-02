using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public class PlantControl : UserControl
    {
        bool translating = false;
        Vector2i mousedown = new Vector2i(0, 0);
        Pen pen = new Pen(Brushes.Gray, 5);

        public Plant Plant { get; set; }

        public PlantControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Plant == null) return;

            foreach (var node in Plant.Nodes)
            {

            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                translating = true;
                mousedown = e.Location;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            translating = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (translating && Plant != null)
            {
                var delta = (Vector2i)e.Location - mousedown;
                mousedown = e.Location;

                foreach (Control child in Controls)
                    child.Location = (Vector2i)child.Location + delta;

            }
        }

    }

}
