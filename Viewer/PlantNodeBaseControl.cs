using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class PlantNodeBaseControl : UserControl
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


        public Color TitleColor
        {
            get => labelTitle.BackColor;
            set => labelTitle.BackColor = value;
        }

        /// <summary>
        /// Designer compatibility
        /// </summary>
        public PlantNodeBaseControl()
        {
            InitializeComponent();
        }
        public PlantNodeBaseControl(PlantNode plantnode)
        {
            InitializeComponent();
            PlantNode = plantnode;
            
            labelTitle.Text = plantnode.Name;

            labelTitle.MouseDown += mouseDown;
            labelTitle.MouseUp += mouseUp;
            labelTitle.MouseMove += mouseMove;
            MouseDown += mouseDown;
            MouseUp += mouseUp;
            MouseMove += mouseMove;
        }




        void mouseDown(object sender, MouseEventArgs e)
        {
            OnClickDown?.Invoke(this);
            if (e.Button == MouseButtons.Left)
            {
                traslating = true;
                mousedown = e.Location;
            }
        }
        void mouseUp(object sender, MouseEventArgs e)
        {
            traslating = false;
        }
        void mouseMove(object sender, MouseEventArgs e)
        {
            if (traslating)
            {
                int movex = e.Location.X - mousedown.X;
                int movey = e.Location.Y - mousedown.Y;

                Location = new Point(Location.X + movex, Location.Y + movey);
            }
        }
        void buttonClose_Click(object sender, EventArgs e)
        {
            PlantNode.Plant.RemoveFactoryNode(PlantNode);
            OnClosing?.Invoke(this);
        }
    }
}
