using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public interface ILinkable
    {
        void DoLink(Link other);
        void UnLink();
    }

    /// <summary>
    /// GUI interface for <see cref="Link"/> object
    /// </summary>
    [DisplayName("{Node}")]
    public class LinkControl : UserControl , ILinkable
    {
        ToolTip info;

        public static LinkControl CurrentSelected;
        bool selected = false;
        public static Vector2i PreferedSize = new Vector2i(70, 34);
        public NodeControl Owner;
        public PlantControl Main => Owner.Owner;
        public Link Node { get; }
        public LinkControl Linked
        { 
            get 
            {
                if (Node.Linked != null)
                    if (Node.Linked.Controller is LinkControl linked)
                        return linked;
                return null;
            } 
        }
        public bool Selected
        {
            get => selected;
            set
            {
                if (value != selected)
                    Invalidate();
                selected = value;
            }
        }
        public bool Mirrored => Owner != null ? Owner.Mirrored : false;

        public LinkControl(Link node, NodeControl owner, bool prioritybox = false)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = PreferedSize;
            Node = node;
            node.Controller = this;
            Owner = owner;

            if (prioritybox)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Checked = node.Priority;
                checkbox.Location = new Point(Width - 16, 0);
                checkbox.Name = "radioButton1";
                checkbox.BackColor = Color.Transparent;
                checkbox.CheckedChanged += CheckedChanged;
                Controls.Add(checkbox);
            }

            info = new ToolTip();
            
        }
        public (Vector2i pointA, Vector2i pointB) ConnectionLine
        {
            get
            {
                int x0 = 0;
                int x1 = 0;
                int y = (Top + Bottom) / 2;

                if (Mirrored ^ Node.Type == LinkType.Input)
                {
                    x0 = Left;
                    x1 = x0 - 20;
                }
                else
                {
                    x0 = Right;
                    x1 = x0 + 20;
                }
                return (new Vector2i(x0, y), new Vector2i(x1, y));
            }
        }
        public void DoLink(Link other)
        {
            if (other.Controller is LinkControl linked)
                Main.AddConnection(this, linked);
        }
        public void UnLink()
        {
            Main.RemoveConnection(this);
        }
        private void CheckedChanged(object sender, System.EventArgs e)
        {
            Node.Priority = ((CheckBox)sender).Checked;
        }

        protected override void OnMouseHover(EventArgs e)
        {
            info.SetToolTip(this, $"enter: {Node.Entering} i/min\nforward: {Node.Forward} i/min");
            base.OnMouseHover(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                Node.UnLink();
                Selected = true;

                if (CurrentSelected != null)
                {
                    Selected = false;
                    CurrentSelected.Selected = false;
                    Node.DoLink(CurrentSelected.Node);
                    CurrentSelected = null;
                }
                else
                {
                    CurrentSelected = this;
                }
                Invalidate();
            }

        }
        protected override void OnPaint(PaintEventArgs e)
        {

            if (Node.Linked != null)
                BackColor = ColorTranslator.FromHtml("#33FF33");
            else
                BackColor = selected ? ColorTranslator.FromHtml("#FFFF99") : Color.White;


            base.OnPaint(e);

            string resource_Text = $"{Node.Resource.Name}";
            if (Node.Quantity>0) resource_Text = $"{Node.Quantity}\n" + resource_Text;

            e.Graphics.DrawString(resource_Text, ControlManager.ArialBold6, Brushes.Black, 2, 2);
            
            e.Graphics.DrawString(Node.Forward.ToString(), ControlManager.ArialBold6, Brushes.Black, 2, 18);

            var srcRect = IconsManager.GetRectangle(Node.Resource);
            var destRect = srcRect;
            destRect.X = Width - srcRect.Width;
            destRect.Y = 0;
            
            e.Graphics.DrawImage(IconsManager.Image, destRect, srcRect, GraphicsUnit.Pixel);

            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }
    }
}
