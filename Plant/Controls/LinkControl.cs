using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CaptainOfPlanner.Controls
{
    public delegate void LinkMouseHandler(LinkControl sender, MouseEventArgs mouse);

    /// <summary>
    /// Base control, define the resources linkers between Nodes
    /// </summary>
    public class LinkControl : UserControl
    {
        Link node;
        bool selected = false;
        bool mirror = false;
        public static Vector2i MyDefaultSize = new Vector2i(70, 25);

        protected static Font font;
        protected static SolidBrush brush;
        protected static Pen pen;

        public bool Mirror
        {
            get => mirror;
            set
            {
                mirror = value;
                SetConnectorOffsets(node.Type, mirror);
            }
        }
        public Link Node
        {
            get => node;
            set
            {
                node = value;
                SetConnectorOffsets(node.Type, mirror);
            }
        }
        public bool Selected 
        {
            get => selected;
            set 
            {
                if (value!= selected)
                {
                    Invalidate(); 
                }
                selected = value;
            }
        }


        public Vector2i LineConnectorOffset = new Vector2i(0, MyDefaultSize.height / 2);
        
        
        void SetConnectorOffsets(LinkType type, bool mirror)
        {
            LineConnectorOffset.x = 0;
            if (type == LinkType.Output || mirror)
                LineConnectorOffset.x = MyDefaultSize.width;
        }



        static LinkControl()
        {
            brush = new SolidBrush(Color.Gray);
            font = new Font("Arial", 6f);
            pen = Pens.Black;
        }


        internal event LinkMouseHandler LinkMouseDown;
        internal event LinkMouseHandler LinkMouseUp;
        internal event LinkMouseHandler LinkMouseClick;

        public LinkControl()
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.UserPaint, true);
            Size = MyDefaultSize;
            Top = 0;
            Left = 0;
        }

       

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
                LinkMouseClick?.Invoke(this, e);
            
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Node.Linked != null)
                BackColor = Color.Green;
            else
                BackColor = selected ? Color.Tan : Color.White;
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var res = Node.ResourceCount;
            base.OnPaint(e);

            //e.Graphics.Clear(selected ? Color.Tan : Color.White);
            e.Graphics.DrawString($"{res.Count}\n{res.Resource.Name}", font, Brushes.Black, 2, 2);
            
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
