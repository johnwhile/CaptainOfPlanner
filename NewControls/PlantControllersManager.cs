
using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public class PlantControllersManager
    {  
        UserControl Viewer;
        Plant plant;

        public Plant Plant
        {
            get => plant;
            set
            {
                if (plant == value) return;
                if (plant != null)
                {
                    plant.OnNodeCreating -= PlantOnNodeCreating;
                    plant.OnNodeRemoving -= PlantOnNodeRemoving;
                    Viewer.Controls.Clear();
                }
                if (value != null)
                {
                    value.OnNodeCreating += PlantOnNodeCreating;
                    value.OnNodeRemoving += PlantOnNodeRemoving;
                }
                plant = value;
            }
        }
      

        /// <summary>
        /// </summary>
        /// <param name="viewer">must be an empty controller</param>
        public PlantControllersManager(UserControl viewer)
        {
            if (viewer == null) throw new ArgumentNullException();
            Viewer = viewer;
        }


        public bool CreateControl(Node node, out Control control)
        {
            control = null;
            if (node is Processor processor)
            {
                ProcessControl control_proc = new ProcessControl(processor);
                control = control_proc;

            }





            return true;
        }



        private void PlantOnNodeCreating(Node sender)
        {
            throw new NotImplementedException();
        }

        private void PlantOnNodeRemoving(Node sender)
        {
            throw new NotImplementedException();
        }
    }
}
