using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    /// <summary>
    /// Base GUI of <see cref="CaptainOfPlanner.Node"/>
    /// </summary>
    [DisplayName("{Node}")]
    public partial class NodeControl : UserControl
    {
        #region parameters
        public static Font ArialBold6;
        public static SolidBrush SBrush;
        public static Pen BlackPen;
        protected static int instance_count = 0;
        protected const int HeaderHeight = 20;
        protected static Vector2i preferedsize;
        bool draging;
        bool mirrored;
        Vector2i mousedown;

        public PlantControl Owner;
        public virtual Node Node { get; }
        public Color NodeColor { get; set; }
        public int Id { get; }

        protected Vector2i OffsetInput;
        protected Vector2i OffsetOutput;

        public bool Mirrored
        {
            get => mirrored;
            private set
            {
                if (mirrored!=value)
                {
                    SuspendLayout();
                    Extensions.Swap(ref OffsetInput, ref OffsetOutput);

                    foreach (var control in Controls.OfType<LinkControl>().ToList())
                    {
                        control.Left = control.Node.Type == LinkType.Input ?
                            OffsetInput.x :
                            OffsetOutput.x;
                    }

                    ResumeLayout(true);
                    Parent?.Invalidate();
                    Invalidate();
                }
                mirrored = value;
            }
        }

        #endregion


        static NodeControl()
        {
            preferedsize = new Vector2i(175, 75);
            SBrush = new SolidBrush(Color.Gray);
            ArialBold6 = new Font("Arial", 6f, FontStyle.Bold);
            BlackPen = Pens.Black;
        }

        /// <summary>
        /// For Designer mode
        /// </summary>
        public NodeControl() : this(null, null) { }

        public NodeControl(Node node = null, PlantControl owner = null)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            NodeColor = ColorTranslator.FromHtml("#606060");
            draging = false;
            mirrored = false;
            Id = instance_count++;
            Owner = owner;

            //fix layout
            Size = preferedsize;
            InitializeComponent();
            buttonClose.Location = new Point(Width - 21 - 2, 3);
            buttonMirror.Location = new Point(Width - 21 - 21 - 4, 3);


            OffsetInput = new Vector2i(2, HeaderHeight + 2);
            OffsetOutput = new Vector2i(Width - LinkControl.PreferedSize.width - 4, HeaderHeight + 2);

            if (DesignMode || node == null) return;
            Location = node.Position;
            Name = node.Name;
            Mirrored = node.Mirrored;
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
        /// <summary>
        /// Create the list of Inputs and Outputs link controllers
        /// </summary>
        protected void CreateLinkControls(LinkCollection inputs, LinkCollection outputs)
        {
            int height = preferedsize.height;

            Vector2i pos = OffsetInput;
            foreach (var link in inputs)
            {
                var ctrl = new LinkControl(link, this);
                ctrl.Location = pos;
                Controls.Add(ctrl);
                pos.y += ctrl.Size.Height + 2;
            }

            height = Math.Max(height, pos.y + 5);

            pos = OffsetOutput;
            foreach (var link in outputs)
            {
                var ctrl = new LinkControl(link, this);
                ctrl.Location = pos;
                Controls.Add(ctrl);
                pos.y += ctrl.Size.Height + 2;
            }
            height = Math.Max(height, pos.y + 5);
            Height = height;
        }


        #region events
        private void buttonMirror_Click(object sender, EventArgs e) => Mirrored = !Mirrored;
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Node.Plant.RemoveNode(Node);
            Owner.RemoveNodeControl(this);
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
                Location = (Vector2i)Location + ((Vector2i)e.Location - mousedown);
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

            SBrush.Color = NodeColor;

            e.Graphics.FillRectangle(SBrush, 0, 0, Width, HeaderHeight);
            e.Graphics.DrawString($"{Id} {Name}", ArialBold6, Brushes.Black, 5, 5);

            BackColor = Color.White;
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }
        #endregion
    }
}
