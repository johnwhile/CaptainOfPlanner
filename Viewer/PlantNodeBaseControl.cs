using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public abstract partial class PlantNodeBaseControl : UserControl
    {
        public delegate void PlantNodeHandler(PlantNodeBaseControl sender);

        bool focused = false;
        bool traslating = false;
        Point mousedown;
        PlantNode PlantNode;
        
        public event PlantNodeHandler OnClosing;
        public event PlantNodeHandler OnClickDown;

        public bool Selected
        {
            get => focused;
            set
            {
                if (focused != value)
                {
                    focused = value;
                    if (focused)
                    {
                        BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        BorderStyle = BorderStyle.None;
                    }
                }
            }
        }


        public PlantNodeBaseControl(PlantNode plantnode)
        {
            InitializeComponent();
            PlantNode = plantnode;
            BackColor = plantnode.BackColor;
            label1.Text = plantnode.Name;

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            OnClickDown?.Invoke(this);

            if (e.Button == MouseButtons.Left)
            {
                traslating = true;
                mousedown = e.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            traslating = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (traslating)
            {
                int movex = e.Location.X - mousedown.X;
                int movey = e.Location.Y - mousedown.Y;

                Location = new Point(Location.X + movex, Location.Y + movey);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            PlantNode.Plant.RemoveFactoryNode(PlantNode);
            OnClosing?.Invoke(this);
        }
    }
}
