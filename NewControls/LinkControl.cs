using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public delegate void LinkMouseHandler(LinkControl sender, MouseEventArgs mouse);

    public interface ILinkable
    {
        void DoLink(Link other);
        void UnLink();
    }


    /// <summary>
    /// Base control, define the resources linkers between Nodes
    /// </summary>
    public class LinkControl : UserControl , ILinkable
    {
        public static LinkControl CurrentSelected;

        bool selected = false;
        bool linked = false;

        protected static Vector2i preferedsize = new Vector2i(70, 25);
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
                {
                    Invalidate();
                }
                selected = value;
            }
        }

        internal event LinkMouseHandler LinkMouseDown;
        internal event LinkMouseHandler LinkMouseUp;
        internal event LinkMouseHandler LinkMouseClick;

        public LinkControl(Link node)
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.UserPaint, true);
            Size = preferedsize;
            Node = node;
            node.Controller = this;
        }


        public void DoLink(Link other)
        {
            // already linked
            if (Node.Linked == other) return;
            
            if (Node.Type == LinkType.Input && Parent?.Parent is PlantControl plantcontrol)
            {
                if (other.Controller is LinkControl linked)

                plantcontrol.AddConnection(this, linked);
            }
        }

        public void UnLink()
        {
            linked = false;

            if (Node.Type== LinkType.Input && Parent?.Parent is PlantControl plantcontrol)
            {
                plantcontrol.RemoveConnection(this);
            }
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
                LinkMouseClick?.Invoke(this, e);
            }

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Node.Linked != null)
                BackColor = ColorTranslator.FromHtml("#33FF33");
            else
                BackColor = selected ? ColorTranslator.FromHtml("#FFFF99") : Color.White;
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var res = Node.ResourceCount;
            base.OnPaint(e);

            e.Graphics.DrawString($"{res.Count}\n{res.Resource.Name}", ControlManager.ArialBold6, Brushes.Black, 2, 2);
            ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised);
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
