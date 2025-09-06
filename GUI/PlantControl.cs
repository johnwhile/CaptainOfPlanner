using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    /// <summary>
    /// GUI interface for <see cref="Plant"/> graph
    /// </summary>
    public class PlantControl : UserControl
    {
        Dictionary<LinkControl, LinkControl> ConnectTo;

        bool translating = false;
        Vector2i mousedown = new Vector2i(0, 0);
        Pen pen = new Pen(Brushes.Gray, 5);

        public Plant Plant { get; set; }

        public PlantControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            ConnectTo = new Dictionary<LinkControl, LinkControl>();
        }

        public LinkControl CurrentSelected;

        public void RemoveNodeControl(UserControl node)
        {
            Controls.Remove(node);
            Invalidate();
        }

        public void RemoveConnection(LinkControl linkctrl)
        {
            Console.WriteLine("Remove Connection " + linkctrl);

            if (ConnectTo.TryGetValue(linkctrl, out LinkControl tolink))
                tolink.Invalidate();

            ConnectTo.Remove(linkctrl);
            linkctrl.Invalidate();

            Invalidate();
        }
        public void AddConnection(LinkControl fromlink, LinkControl tolink)
        {
            Console.WriteLine("Add Connection from " + fromlink + " to " + tolink);

            ConnectTo.Add(fromlink, tolink);
            fromlink.Invalidate();
            tolink.Invalidate();
            Invalidate();
        }

        public void AddNewNodeAndControler(NodeType type)
        {
            if (Plant == null) return;

            Control controller;
            switch(type)
            {
                case NodeType.Processor:
                    controller = new ProcessControl((Processor)Plant.GenerateNode(type), this); break;
                case NodeType.Balancer:
                    controller = new BalanceControl((Balancer)Plant.GenerateNode(type), this); break;
                case NodeType.Storage:
                    controller = new StorageControl((Storage)Plant.GenerateNode(type), this); break;
                default: throw new Exception("unknow node class");

            }
            if (controller != null) Controls.Add(controller);
            Invalidate();
        }

        public void GenerateControllers()
        {
            if (Plant == null) return;

            SuspendLayout();
            Controls.Clear();

            foreach (Node node in Plant)
            {
                Control controller;
                if (node is Processor processor) controller = new ProcessControl(processor, this);
                else if (node is Balancer balancer) controller = new BalanceControl(balancer, this);
                else if (node is Storage storage) controller = new StorageControl(storage, this);
                else controller = null;

                if (controller != null) Controls.Add(controller);
                else throw new Exception("unknow node class");
            }
            ResumeLayout(true);

            ConnectTo.Clear();
            foreach (Node node in Plant)
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

        Point[] line4 = new Point[4];

        protected override void OnPaint(PaintEventArgs e)
        {
            Vector2i PointToClient(Vector2i point, Control owner) =>
                this.PointToClient(owner.Parent.PointToScreen(point));

            base.OnPaint(e);
            if (Plant == null) return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var pair in ConnectTo)
            {
                LinkControl input = pair.Key;
                LinkControl output = pair.Value;

                var line0 = input.ConnectionLine;
                var p0 = PointToClient(line0.pointA, input);
                var p1 = PointToClient(line0.pointB, input);

                var line1 = output.ConnectionLine;
                var p2 = PointToClient(line1.pointA, output);
                var p3 = PointToClient(line1.pointB, output);

                //e.Graphics.DrawLine(pen, p0, p2);
                /*
                line4[0] = p0;
                line4[1] = p1;
                line4[2] = p3;
                line4[3] = p2;
                e.Graphics.DrawLines(pen, line4);
                */
                int dist = Math.Max(50, Math.Abs(p0.x - p2.x));
                p1.x = p0.x + Math.Sign(p1.x - p0.x) * dist;
                p3.x = p2.x + Math.Sign(p3.x - p2.x) * dist;
                e.Graphics.DrawBezier(pen, p0, p1, p3, p2);

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
