using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{
    public class ButtonClose : Button
    {
        public ButtonClose()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackgroundImage = Properties.Resources.close;
            FlatStyle = FlatStyle.Standard;
            Size = BackgroundImage.Size;


        }
    }

    /// <summary>
    /// The base class <see cref="NodeControl"/> can be used as generic node
    /// </summary>
    public class NodeControl : UserControl
    {
        public delegate void NodeControlHandler(NodeControl sender);
        public event NodeControlHandler OnClosing;

        
        protected static Font font;
        protected static SolidBrush brush;
        protected static Pen pen;
        protected bool draging;
        protected Point mousedown;
        protected ButtonClose buttonclose;
        protected int headerHeight = 20;
        
        static NodeControl()
        {
            brush = new SolidBrush(Color.Gray);
            font = new Font("Arial", 6f);
            pen = Pens.Black;
           
        }

        public List<InputLinkControl> Inputs { get; }
        public List<OutputLinkControl> Outputs { get; }
        public PlantControl PlantControl { get; }
        public Color HeaderColor { get; set; }
        public Node Node { get; }

        public NodeControl(PlantControl owner, Node node)
        {
            PlantControl = owner;
            Node = node;
            Name = node.Name;
            Inputs = new List<InputLinkControl>();
            Outputs = new List<OutputLinkControl>();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            Width = 150;
            Height = 100;
            Top = 0;
            Left = 0;

            buttonclose = new ButtonClose();
            buttonclose.Location = new Point(Width - buttonclose.Size.Width - 2, 2);
            buttonclose.Click += Close_Click;
            Controls.Add(buttonclose);

            draging = false;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            OnClosing?.Invoke(this);
            PlantControl.Controls.Remove(this);
            Node.Plant.RemoveNode(Node);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            BringToFront();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            brush.Color = HeaderColor;
            
            e.Graphics.FillRectangle(brush, 0, 0, Size.Width, headerHeight);
            e.Graphics.DrawString(Name, font, Brushes.Black, 5, 5);

            BackColor = Color.White;
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Inputs.Clear();
            Outputs.Clear();

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) draging = true;
            mousedown = e.Location;
            //Console.WriteLine($"DOWN {e.X} {e.Y}");
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (draging)
            {
                Location = Location.Sum(e.Location.Sub(mousedown));

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
}
