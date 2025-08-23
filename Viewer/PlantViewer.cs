using System;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class PlantViewer : UserControl
    {
        public Window Main;
        PlantNodeBaseControl CurrentSelected;


        public PlantViewer()
        {
            InitializeComponent();
            /*
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            */
        }

        public PlantNodeBaseControl AddPlantNode(PlantNode node)
        {
            PlantNodeBaseControl controller = null;

            if (node is ProcessorNode pnode)
            {
                controller = new ProcessorControl(pnode);
            }
            else if (node is ContainerNode cnode)
            {
                controller = new ContainerControl(cnode);
            }
            else if (node is BalancerNode bnode)
            {
                controller = new BalancerControl(bnode);
            }

            SuspendLayout();
            Controls.Add(controller);
            ResumeLayout();

            controller.OnClosing += PlantNodeController_Closing;
            controller.OnClickDown += PlantNodeController_Selected;
            return controller;
        }
        private void PlantNodeController_Selected(PlantNodeBaseControl sender)
        {
            if (CurrentSelected != null && CurrentSelected != sender) 
                CurrentSelected.Selected = false;
            CurrentSelected = sender;
            CurrentSelected.Selected = true;
        }
        private void PlantNodeController_Closing(PlantNodeBaseControl sender)
        {
            SuspendLayout();
            Controls.Remove(sender);
            sender.OnClosing-= PlantNodeController_Closing;
            sender.OnClickDown -= PlantNodeController_Selected;
            ResumeLayout();
        }
    }
}
