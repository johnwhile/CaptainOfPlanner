using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{
    public class PlantControl : UserControl
    {
        bool translating = false;
        Vector2i mousedown = new Vector2i(0, 0);
        Pen pen = new Pen(Brushes.Gray, 5);

        public Plant Plant { get; set; }

        Timer timer;

        /// <summary>
        /// ready to link to another linker
        /// </summary>
        public LinkControl Selected { get; set; }

        public PlantControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            timer = new Timer();
            timer.Interval = 200;
            //timer.Tick += Timer_Tick;
            timer.Start();
        }

        public event NodeHandler OnNodeClosing;

        public void BringToFront(NodeControl node)
        {
            node.BringToFront();
            //Overlay?.BringToFront();
        }
        

        public void Clear()
        {
            Controls.Clear();
        }

        public void AddNodeControl(NodeControl node)
        {
            SuspendLayout();
            Controls.Add(node);
            node.OnLinkerClick += OnLinkerClick;
            node.OnClosing += NodeClosing;

            //Overlay?.BringToFront();
            ResumeLayout(true);
        }
        public void RemoveNodeControl(NodeControl node)
        {
            SuspendLayout();
            Controls.Remove(node);
            node.OnLinkerClick -= OnLinkerClick;
            node.OnClosing -= NodeClosing;
            //Overlay?.BringToFront();
            ResumeLayout(true);
        }
        private void NodeClosing(NodeControl sender)
        {
            OnNodeClosing?.Invoke(sender.Node);
            Invalidate();
        }
        private void OnLinkerClick(LinkControl sender, MouseEventArgs mouse)
        {
            if (Plant == null) return;
            if (Selected == null)
            {
                sender.Selected = true;
                Selected = sender;
            }
            else
            {
                Selected.Selected = false;
                Plant.Linking(Selected.Node, sender.Node);
                Selected.Invalidate();
                sender.Invalidate();
                Selected = null;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            timer.Stop();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
            //Console.WriteLine("tick");
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Plant == null) return;
            foreach (var node in Plant.Nodes)
            {
                foreach (var linkA in node.Outputs)
                {
                    var linkB = linkA.Linked;
                    if (linkB == null) continue;

                    Vector2i start = (Vector2i)linkA.Control.Location + linkA.Control.LineConnectorOffset;
                    Vector2i end = (Vector2i)linkB.Control.Location + linkB.Control.LineConnectorOffset;

                    start = PointToClient(linkA.Control.Parent.PointToScreen(start));
                    end = PointToClient(linkB.Control.Parent.PointToScreen(end));

                    e.Graphics.DrawLine(pen, start, end);
                }
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
            if (translating && Plant!=null)
            {
                var delta = (Vector2i)e.Location - mousedown;
                mousedown = e.Location;

                foreach (var node in Plant.Nodes)
                    if (node.Control != null)
                    {
                        node.Control.Location = (Vector2i)node.Control.Location + delta;
                    }
                Invalidate();
            }
        }

    }

    public delegate void NodeHandler(Node sender);
}
