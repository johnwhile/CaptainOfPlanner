using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Windows.Forms;


namespace CaptainOfPlanner.Controls
{
    /// <summary>
    /// The base class <see cref="NodeControl"/> can be used as generic node
    /// </summary>
    public class NodeControl : UserControl
    {
        public event NodeControlHandler OnClosing;
        public event LinkMouseHandler OnLinkerClick;

        protected static Font font;
        protected static SolidBrush brush;
        protected static Pen pen;
        protected bool draging;
        protected Vector2i mousedown;
        protected ButtonClose buttonclose;
        protected int headerHeight = 20;

        protected Vector2i MyDefaultSize = new Vector2i(150, 50);

        static NodeControl()
        {
            brush = new SolidBrush(Color.Gray);
            font = new Font("Arial", 6f);
            pen = Pens.Black;

        }
        public List<LinkControl> Inputs { get; }
        public List<LinkControl> Outputs { get; }
        public PlantControl PlantControl => Parent is PlantControl plant ? plant : null;       
        public Color HeaderColor { get; set; }
        public Node Node { get; }


        public NodeControl(Node node)
        {
            Node = node;
            Name = node.Name;
            Inputs = new List<LinkControl>();
            Outputs = new List<LinkControl>();

            //SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            BackColor = Color.Transparent;
            Top = 0;
            Left = 0;
            Height = MyDefaultSize.height;
            Width = MyDefaultSize.width;

            buttonclose = new ButtonClose();
            buttonclose.Location = new Point(Width - buttonclose.Size.Width - 2, 2);
            buttonclose.Click += Close_Click;
            Controls.Add(buttonclose);

            draging = false;
        }

        void FixSize()
        {
            int height = MyDefaultSize.height;
            int width = MyDefaultSize.width;

            foreach (var obj in Controls)

                if (obj is Control control)
                {
                    height = Math.Max(height, control.Bottom + 2);
                    width = Math.Max(width, control.Right + 2);
                }
            Height = height;
            Width = width;
        }


        /// <summary>
        /// Add link controller to Controls
        /// </summary>
        /// <param name="offset">set the Top-Left corner of first link</param>
        public void AddLinkControl(LinkControl link, Vector2i offset)
        {
            SuspendLayout();
            
            link.LinkMouseClick -= LinkMouseClick;
            link.LinkMouseClick += LinkMouseClick;

            List<LinkControl> list = link.Node.Type == LinkType.Input ? Inputs : Outputs;
            LinkControl last = list.GetLast();
            list.Add(link);

            if (last != null) offset.y = last.Bottom + 2;
            link.Location = offset;

            Controls.Add(link);
            FixSize();
            ResumeLayout(true);
        }


        void removeControl(LinkControl link)
        {
            Controls.Remove(link);
            link.LinkMouseClick -= LinkMouseClick;
            if (link.Node.Type == LinkType.Input) Inputs.Remove(link);
            if (link.Node.Type == LinkType.Output) Outputs.Remove(link);

        }
        public void RemoveLinkControl(LinkControl link)
        {
            SuspendLayout();
            removeControl(link);
            FixSize();
            ResumeLayout(true);
        }
        public void RemoveLinkControl(IEnumerable<LinkControl> list)
        {
            SuspendLayout();
            foreach (var link in list) removeControl(link);
            FixSize();
            ResumeLayout(true);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            OnClosing?.Invoke(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
           
            brush.Color = HeaderColor;
            
            e.Graphics.FillRectangle(brush, 0, 0, Width, headerHeight);
            e.Graphics.DrawString(Name, font, Brushes.Black, 5, 5);

            BackColor = Color.White;
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        private void LinkMouseClick(LinkControl sender, MouseEventArgs mouse)
        {
            OnLinkerClick?.Invoke(sender, mouse);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            PlantControl?.BringToFront(this);
            if (e.Button == MouseButtons.Left) draging = true;
            mousedown = e.Location;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (draging)
            {
                Location = (Vector2i)Location + ((Vector2i)e.Location - mousedown);
                Parent.Invalidate();
                //int movex = e.Location.X - mousedown.X;
                //int movey = e.Location.Y - mousedown.Y;
                //Location = new Point(Location.X + movex, Location.Y + movey);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            draging = false;
        }


    }
    
    public delegate void NodeControlHandler(NodeControl sender);

}
