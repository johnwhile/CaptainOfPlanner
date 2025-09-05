using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptainOfPlanner
{

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
        Vector2i mousedown;

        public virtual Node Node { get; }
        public Color NodeColor { get; set; }
        public int Id { get; }
        public new Vector2i Location
        {
            get => base.Location;
            set { base.Location = value; Node.Position = value; }
        }
        protected Vector2i OffsetInput;
        protected Vector2i OffsetOutput;



        #endregion


        static NodeControl()
        {
            preferedsize = new Vector2i(150, 75);
            SBrush = new SolidBrush(Color.Gray);
            ArialBold6 = new Font("Arial", 6f, FontStyle.Bold);
            BlackPen = Pens.Black;
        }

        public NodeControl(Node node = null)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            NodeColor = ColorTranslator.FromHtml("#606060");
            draging = false;
            Id = instance_count++;
            Name = "NodeCtrl";

            if (node!=null) base.Location = node.Position;

            InitializeComponent();

            OffsetInput = new Vector2i(2, HeaderHeight + 2);
            OffsetOutput = new Vector2i(Width - LinkControl.PreferedSize.width - 4, HeaderHeight + 2);
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
                var ctrl = new LinkControl(link);
                ctrl.Location = pos;
                Controls.Add(ctrl);
                pos.y += ctrl.Size.Height + 2;
            }

            height = Math.Max(height, pos.y + 5);

            pos = OffsetOutput;
            foreach (var link in outputs)
            {
                var ctrl = new LinkControl(link);
                ctrl.Location = pos;
                Controls.Add(ctrl);
                pos.y += ctrl.Size.Height + 2;
            }
            height = Math.Max(height, pos.y + 5);
            Height = height;
        }
       
        
        
       
        #region events
        private void buttonMirror_Click(object sender, EventArgs e)
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

        private void buttonClose_Click(object sender, EventArgs e)
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

            SBrush.Color = NodeColor;

            e.Graphics.FillRectangle(SBrush, 0, 0, Width, HeaderHeight);
            e.Graphics.DrawString($"{Id} {Name}", ArialBold6, Brushes.Black, 5, 5);

            BackColor = Color.White;
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }
        #endregion
    }
}
