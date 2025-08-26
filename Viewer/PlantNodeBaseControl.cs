using System;
using System.Collections.Generic;
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

        protected List<LinkerControl> InputControls;
        protected List<LinkerControl> OutputControls;

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

            InputControls = new List<LinkerControl>();
            OutputControls = new List<LinkerControl>();

            PlantNode = plantnode;
            Name = plantnode.Name;
            labelTitle.Text = Name;

            labelTitle.MouseDown += mouseDown;
            labelTitle.MouseUp += mouseUp;
            labelTitle.MouseMove += mouseMove;
            MouseDown += mouseDown;
            MouseUp += mouseUp;
            MouseMove += mouseMove;
        }

        /// <summary>
        /// remove inputs and outputs controls
        /// </summary>
        protected void RemoveLinkers()
        {
            SuspendLayout();
            foreach (var linker in InputControls) Controls.Remove(linker);
            foreach (var linker in OutputControls) Controls.Remove(linker);
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// add inputs and outputs controls
        /// </summary>
        protected void ResumeLinkers()
        {
            SuspendLayout();
            foreach (var linker in InputControls) Controls.Add(linker);
            foreach (var linker in OutputControls) Controls.Add(linker);
            ResumeLayout(false);
            PerformLayout();
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

        public override string ToString()
        {
            return Name;
        }
    }
}
