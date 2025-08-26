using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{
    public class PlantControl : UserControl
    {
        public Plant Plant { get; }

        public PlantControl(Plant plant)
        {
            Plant = plant;
        }

    }
}
