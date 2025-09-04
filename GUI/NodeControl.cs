using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptainOfPlanner
{

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class NodeControl : UserControl
    {
        static int instance_count = 0;
        protected const int HeaderHeight = 20;
        protected static Vector2i preferedsize = new Vector2i(150, 50);

        bool draging;
        Vector2i mousedown;
        ButtonClose buttonclose;
        
        public new Vector2i Location
        {
            get => base.Location;
            set { base.Location = value; Node.Position = value; }
        }

        public Color NodeColor { get; set; }
        public int Id { get; }
        public abstract Node Node { get; }

        public NodeControl(Node node)
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

            base.Location = node.Position;
        }

        /// <summary>
        /// Remove only LinkControl from controls
        /// </summary>
        protected void RemoveLinkControls()
        {
            foreach (var control in Controls.OfType<LinkControl>().ToList())
            {
                control.Node.UnLink();
                Controls.Remove(control);
            }
        }
        protected void CreateLinkControls(int offsety, LinkCollection inputs, LinkCollection outputs)
        {
            int height = preferedsize.height;

            Vector2i pos = new Vector2i(2, offsety + 10);
            foreach (var link in inputs)
            {
                var ctrl = new LinkControl(link);
                ctrl.Location = pos;
                Controls.Add(ctrl);
                pos.y += ctrl.Size.Height + 2;
            }

            height = Math.Max(height, pos.y + 5);

            pos = new Vector2i(0, offsety + 10);
            foreach (var link in outputs)
            {
                var ctrl = new LinkControl(link);
                pos.x = Width - ctrl.Width - 2;
                ctrl.Location = pos;
                Controls.Add(ctrl);
                pos.y += ctrl.Size.Height + 2;
            }
            height = Math.Max(height, pos.y + 5);
            Height = height;
        }
        private void OnCloseClick(object sender, EventArgs e)
        {
            Node.Plant.RemoveNode(Node);
            if (Parent is PlantControl plantctrl) 
                plantctrl.RemoveNodeControl(this);
        }

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
                Location = Location + ((Vector2i)e.Location - mousedown);
                Parent.Invalidate();
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

            ControlManager.SBrush.Color = NodeColor;

            e.Graphics.FillRectangle(ControlManager.SBrush, 0, 0, Width, HeaderHeight);
            e.Graphics.DrawString($"{Id} {Name}", ControlManager.ArialBold6, Brushes.Black, 5, 5);

            BackColor = Color.White;
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }
    }
}
