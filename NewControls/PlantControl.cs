using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public class PlantControl : UserControl
    {
        Dictionary<LinkControl, LinkControl> ConnectTo;

        bool translating = false;
        Vector2i mousedown = new Vector2i(0, 0);
        Pen pen = new Pen(Brushes.Gray, 5);

        public Plant Plant { get; set; }

        public PlantControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            ConnectTo = new Dictionary<LinkControl, LinkControl>();
        }

        public LinkControl CurrentSelected;

        public void RemoveControl(NodeControl node)
        {
            Controls.Remove(node);
            Invalidate();
        }

        public void RemoveConnection(LinkControl linkctrl)
        {
            if (ConnectTo.TryGetValue(linkctrl, out LinkControl tolink))
                tolink.Invalidate();

            ConnectTo.Remove(linkctrl);
            linkctrl.Invalidate();

            Invalidate();
        }
        public void AddConnection(LinkControl fromlink, LinkControl tolink)
        {
            ConnectTo.Add(fromlink, tolink);
            fromlink.Invalidate();
            tolink.Invalidate();
            Invalidate();
        }

        public void AddNewNodeAndControler(Plant plant, NodeType type)
        {
            Control controller;
            switch(type)
            {
                case NodeType.Processor:
                    controller = new ProcessControl((Processor)plant.GenerateNode(type)); break;
                case NodeType.Balancer:
                    controller = new BalanceControl((Balancer)plant.GenerateNode(type)); break;
                case NodeType.Storage:
                    controller = new StorageControl((Storage)plant.GenerateNode(type)); break;
                default: throw new Exception("unknow node class");

            }
            if (controller != null) Controls.Add(controller);
            Invalidate();
        }

        public void GenerateControllers(Plant plant)
        {
            SuspendLayout();
            Controls.Clear();

            foreach (Node node in plant)
            {
                Control controller;
                if (node is Processor processor)
                {
                    controller = new ProcessControl(processor);
                }
                else if (node is Balancer balancer)
                {
                    controller = new BalanceControl(balancer);
                }
                else
                {
                    controller = null;
                    throw new Exception("unknow node class");
                }
                if (controller != null) Controls.Add(controller);
            }
            ResumeLayout(true);

            ConnectTo.Clear();
            foreach (Node node in plant)
            {
                foreach(Link link in node.Inputs)
                {
                    if (link.Linked!=null && 
                        link.Controller is LinkControl fromlink &&
                        link.Linked.Controller is LinkControl tolink)

                        ConnectTo.Add(fromlink, tolink);
                }
            }
            Invalidate();

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Plant == null) return;

            foreach (var pair in ConnectTo)
            {
                Vector2i start = (Vector2i)pair.Key.Location + (Vector2i)pair.Key.Size / 2;
                Vector2i end = (Vector2i)pair.Value.Location + (Vector2i)pair.Value.Size / 2;

                start = PointToClient(pair.Key.Parent.PointToScreen(start));
                end = PointToClient(pair.Value.Parent.PointToScreen(end));

                e.Graphics.DrawLine(pen, start, end);
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
            if (translating)
            {
                var delta = (Vector2i)e.Location - mousedown;
                mousedown = e.Location;

                foreach (Control child in Controls)
                    child.Location = (Vector2i)child.Location + delta;

                Invalidate();
            }
        }

    }

}
