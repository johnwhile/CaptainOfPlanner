using CaptainOfPlanner.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public delegate void NodeControlHandler(NodeControl sender);

    public abstract class NodeControl : UserControl
    {
        static int instance_count = 0;
        protected const int HeaderHeight = 20;
        protected static Font font;
        protected static SolidBrush brush;
        protected static Pen pen;
        protected static Vector2i preferedsize = new Vector2i(150, 50);

        bool draging;
        Vector2i mousedown;
        
        ButtonClose buttonclose;

        public Color NodeColor;
        
        public event NodeControlHandler OnRequestClosing;

        public int Id { get; }

        static NodeControl()
        {
            brush = new SolidBrush(Color.Gray);
            font = new Font("Arial", 6f, FontStyle.Bold);
            pen = Pens.Black;
            
        }

        public NodeControl()
        {
            Size = preferedsize;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            NodeColor = ColorTranslator.FromHtml("#606060");

            buttonclose = new ButtonClose();
            buttonclose.Location = new Point(Width - buttonclose.Size.Width - 2, 2);
            buttonclose.Click += OnCloseClick;
            Controls.Add(buttonclose);

            draging = false;
            Id = instance_count++;

            Name = "NodeCtrl";
        }

        private void OnCloseClick(object sender, EventArgs e) => OnRequestClosing?.Invoke(this);


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            BringToFront();
            if (e.Button == MouseButtons.Left) draging = true;
            mousedown = e.Location;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (draging)
            {
                Location = (Vector2i)Location + ((Vector2i)e.Location - mousedown);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            draging = false;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            brush.Color = NodeColor;

            e.Graphics.FillRectangle(brush, 0, 0, Width, HeaderHeight);
            e.Graphics.DrawString($"{Id} {Name}", font, Brushes.Black, 5, 5);

            BackColor = Color.White;
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }
    }
}
