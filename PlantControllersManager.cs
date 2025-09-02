using CaptainOfPlanner.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class PlantControllersManager : IDisposable
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
        public PlantControllersManager(UserControl viewer, Plant plant)
        {
            if (viewer == null || plant == null) throw new ArgumentNullException();

            Viewer = viewer;
            Plant = plant;

            Viewer.Controls.Clear();
        }


        public bool CreateControl(Node node, out Control control)
        {
            if (node is Processor processor)
            {
                ProcessorControl control_proc = new ProcessorControl(processor);
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

        public void Dispose()
        {
            if (Plant != null)
            {
                Plant.OnNodeCreating -= PlantOnNodeCreating;
                Plant.OnNodeRemoving -= PlantOnNodeRemoving;
                Plant = null;
            }
        }
    }
}
